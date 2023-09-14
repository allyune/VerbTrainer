using System;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VerbTrainerUser.DTOs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Npgsql;
using Models = VerbTrainerUser.Infrastructure.Data.Models;
using VerbTrainerUser.Domain.Interfaces;
using VerbTrainerUser.Application.Exceptions;
using VerbTrainerUser.Infrastructure.Data.Models;

namespace VerbTrainerUser.Application.Services.JWT
{
	public class JWTService : IJWTService
	{
        private readonly IConfiguration _configuration;
        private readonly ILogger<JWTService> _logger;
        private readonly IAsyncUserRepository _userRepository;
        private readonly IAsyncAccessTokenRepository _accessTokenRepository;
        private readonly IAsyncRefreshTokenRepository _refreshTokenRepository;

        public JWTService(IConfiguration configuration,
                          ILogger<JWTService> logger,
                          IAsyncUserRepository userRepository,
                          IAsyncAccessTokenRepository accessTokenRepository,
                          IAsyncRefreshTokenRepository refreshTokenRepository)
        {
            _configuration = configuration;
            _logger = logger;
            _userRepository = userRepository;
            _accessTokenRepository = accessTokenRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<string> IssueAccessToken(IssueAccessTokenDto loginData)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            int tokenLifeSpan = _configuration.GetValue<int>("JwtSettings:TokenValidityInMinutes");
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
            int userId = (await _userRepository.GetAsync(u => u.Email == loginData.Email)).Id;
           
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,
                          loginData.Email),
                new Claim(JwtRegisteredClaimNames.Sub,
                          loginData.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                          Convert.ToString(userId)),
                new Claim(JwtRegisteredClaimNames.Exp,
                          DateTimeOffset.UtcNow.AddMinutes(tokenLifeSpan)
                                                .ToUnixTimeSeconds()
                                                .ToString())

            };
            var token = new JwtSecurityToken(issuer: _configuration["JwtSettings:Issuer"],
                                             audience: _configuration["JwtSettings:Audience"],
                                             claims: claims,
                                             signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> IssueRefreshToken(LoginDto loginData)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JwtSettings:RefreshKey"]));
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
            Models.User? userInfo = await _userRepository.GetAsync(u => u.Email == loginData.email);
            if (userInfo is null)
            {
                throw new UserDoesNotExistException($"User {loginData.email} does not exist");
            }
            int userId = userInfo.Id;
            double tokenLifeSpan = GetRefreshTokenLifespan(loginData.rememberUser);
            Console.WriteLine($"Refresh token lifespan: {tokenLifeSpan}");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loginData.email),
                new Claim(JwtRegisteredClaimNames.Sub, loginData.email),
                new Claim(JwtRegisteredClaimNames.Jti, Convert.ToString(userId)),
                new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddDays(tokenLifeSpan).ToUnixTimeSeconds().ToString())
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<bool> RevokeRefreshToken(string refreshToken)
        {
            string encodedToken = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(refreshToken));
            try
            {
                await _refreshTokenRepository.AddAsync(
                    new RevokedRefreshToken { Token = encodedToken });
                int res = await _refreshTokenRepository.SaveChangesAsync();

                if (res != 1)
                {
                    return false;
                }
                return true;
            }

            catch (DbUpdateException e)
            {
                if (e.InnerException is PostgresException postgresException) {
                    switch (postgresException.SqlState)
                    {
                        case "23505":
                            _logger.LogInformation($"Refresh Token {refreshToken} already invalid");
                            return true;
                        default:
                            _logger.LogCritical($"Could not invalidate refresh token {refreshToken}", e);
                            return false;
                    }
                }

                return false;
            }
        }

        public async Task<bool> RevokeAccessToken(string accessToken)
        {
            string encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(accessToken));
            try
            {
               await _accessTokenRepository.AddAsync(new RevokedAccessToken { Token = encodedToken });
               int res = await _accessTokenRepository.SaveChangesAsync();
               if (res != 1)
               {
                   return false;
               }
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
                tokenLifeSpan = _configuration.GetValue<double>(
                    "JwtSettings:RefreshTokenLongValidityInDays");
            }
            else
            {
                tokenLifeSpan = _configuration.GetValue<double>(
                    "JwtSettings:RefreshTokenShortValidityInDays");
            }
            return tokenLifeSpan;
        }

        public bool ValidateToken(string token, out ClaimsPrincipal principal)
        {
            var secretkey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["JwtSettings:RefreshKey"]));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration
                              .GetValue<string>("JwtSettings:Issuer"),
                ValidateAudience = true,
                ValidAudience = _configuration
                                .GetValue<string>("JwtSettings:Audience"),
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = secretkey,
                ValidateLifetime = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            principal = tokenHandler.ValidateToken(
                token, tokenValidationParameters, out SecurityToken validToken);

            bool isValidToken = principal != null;
            bool hasEmailClaim = principal?.Claims.SingleOrDefault(
                claim => claim.Type == ClaimTypes.Email) != null;

            return isValidToken && hasEmailClaim;

        }

        //TODO: Implement class Token with fields id and type
        public async Task<bool> IsAccessTokenBlacklisted(string accessToken)
        {
            string encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(accessToken));
            return await _accessTokenRepository.CheckExists(t => t.Token == encodedToken);
        }

        public async Task<bool> IsRefreshTokenBlacklisted(string refreshToken)
        {
            string encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(refreshToken));
            return await _refreshTokenRepository.CheckExists(t => t.Token == encodedToken);
        }

    }
}



