using System;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VerbTrainer.DTOs;
using VerbTrainerAuth.Data;
using VerbTrainerAuth.Models.Domain;
using VerbTrainerAuth.Controllers;
using VerbTrainerAuth.DTOs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Npgsql;
using System.Linq.Expressions;

namespace VerbTrainerAuth.Services
{
	public class JWTService : IJWTService
	{
        private readonly VerbTrainerAuthDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JWTService> _logger;

        public JWTService(VerbTrainerAuthDbContext dbContext, IConfiguration configuration, ILogger<JWTService> logger)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _logger = logger;
        }

        public string IssueAccessToken(IssueAccessTokenDto loginData)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            int tokenLifeSpan = _configuration.GetValue<int>("JwtSettings:TokenValidityInMinutes");
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
            int userId = _dbContext.Users.First(u => u.Email == loginData.Email).Id;
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loginData.Email),
                new Claim(JwtRegisteredClaimNames.Sub, loginData.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Convert.ToString(userId)),
                new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(tokenLifeSpan).ToUnixTimeSeconds().ToString())

            };
            var token = new JwtSecurityToken(issuer: _configuration["JwtSettings:Issuer"], audience: _configuration["JwtSettings:Audience"], claims: claims, signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string IssueRefreshToken(LoginDto loginData)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JwtSettings:RefreshKey"]));
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
            int userId = _dbContext.Users.First(u => u.Email == loginData.email).Id;
            double tokenLifeSpan = GetRefreshTokenLifespan(loginData.rememberUser);
            Console.WriteLine($"Refresh token lifespan: {tokenLifeSpan}");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loginData.email),
                new Claim(JwtRegisteredClaimNames.Sub, loginData.email),
                new Claim(JwtRegisteredClaimNames.Jti, Convert.ToString(userId)),
                new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(tokenLifeSpan).ToUnixTimeSeconds().ToString())
            };
            var token = new JwtSecurityToken(issuer: _configuration["JwtSettings:Issuer"], audience: _configuration["JwtSettings:Audience"], claims: claims, signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool RevokeRefreshToken(string refreshToken)
        {
            string encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(refreshToken));
            try
            {

                _dbContext.RevokedRefreshTokens.Add(new RevokedRefreshToken { Token = encodedToken });
                _dbContext.SaveChanges();
                return true;
            }

            catch (DbUpdateException e)
            {
                if (e.InnerException is PostgresException postgresException) {
                    Console.WriteLine("SAVING EXCEPTION");
                    switch (postgresException.SqlState)
                    {
                        case "23505":
                            _logger.LogInformation($"Refresh Token {refreshToken} already invalid", e);
                            return true;
                        default:
                            _logger.LogCritical($"Could not invalidate refresh token {refreshToken}", e);
                            return false;
                    }
                }
                _logger.LogCritical($"Could not invalidate refresh token {refreshToken}", e);
                return false;
            }
        }

        public bool RevokeAccessToken(string accessToken)
        {
            string encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(accessToken));
            try
            {
                _dbContext.RevokedAccessTokens.Add(new RevokedAccessToken { Token = encodedToken });
                _dbContext.SaveChanges();
                return true;
            }

            catch (DbUpdateException e)
            {
                if (e.InnerException is PostgresException postgresException)
                {
                    Console.WriteLine("SAVING EXCEPTION");
                    switch (postgresException.SqlState)
                    {
                        case "23505":
                            _logger.LogInformation($"Access Token {accessToken} already invalid", e);
                            return true;
                        default:
                            _logger.LogCritical($"Could not invalidate access token {accessToken}", e);
                            return false;
                    }
                }
                _logger.LogCritical($"Could not invalidate access token {accessToken}", e);
                return false;
            }
        }

        public double GetRefreshTokenLifespan(bool rememberUser)
        {
            double tokenLifeSpan;
            if (rememberUser)
            {
                tokenLifeSpan = _configuration.GetValue<double>("JwtSettings:RefreshTokenLongValidityInDays");
            }
            else
            {
                tokenLifeSpan = _configuration.GetValue<double>("JwtSettings:RefreshTokenShortValidityInDays");
            }
            return tokenLifeSpan;
        }

        public bool ValidateToken (string token)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JwtSettings:RefreshKey"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration.GetValue<string>("JwtSettings:Issuer"),
                ValidateAudience = true,
                ValidAudience = _configuration.GetValue<string>("JwtSettings:Audience"),
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = secretkey,
                ValidateLifetime = true
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validToken);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            };
        }

        //TODO: Implement class Token with fields id and type
        public bool IsAccessTokenBlacklisted(string accessToken)
        {
            string encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(accessToken));
            RevokedAccessToken? tokenExists = _dbContext.RevokedAccessTokens.FirstOrDefault(t => t.Token == encodedToken);
            if (tokenExists != null)
            {
                return true;
            }
            return false;
        }

        public bool IsRefreshTokenBlacklisted(string refreshToken)
        {
            string encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(refreshToken));
            RevokedRefreshToken? tokenExists = _dbContext.RevokedRefreshTokens.FirstOrDefault(t => t.Token == encodedToken);
            if (tokenExists != null)
            {
                return true;
            }
            return false;
        }

        public IEnumerable<Claim> GetTokenPrincipal(string token)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration.GetValue<string>("JwtSettings:Issuer"),
                ValidateAudience = true,
                ValidAudience = _configuration.GetValue<string>("JwtSettings:Audience"),
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = secretkey,
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                IEnumerable<Claim> claims = principal.Claims;
                return claims;
        }

        public bool IsTokenExpired(string token)
        {
            var tokenClaims = GetTokenPrincipal(token);
            var tokenExpiration = tokenClaims.SingleOrDefault(claim => claim.Type == "exp")?.Value;
            if (tokenExpiration == null || DateTimeOffset.UtcNow.ToUnixTimeSeconds() > long.Parse(tokenExpiration))
            {
                return true;
            }
            return false;
        }
    }
}



