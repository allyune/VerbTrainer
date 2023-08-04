using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VerbTrainer.DTOs;
using VerbTrainerAuth.Data;
using VerbTrainerAuth.Models.Domain;
using VerbTrainerAuth.Controllers;

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

        public string IssueAccessToken(LoginDto loginData)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            int tokenLifespan = _configuration.GetValue<int>("JwtSettings:TokenValidityInMinutes");
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
            int userId = _dbContext.Users.First(u => u.Email == loginData.email).Id;
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loginData.email),
                new Claim(JwtRegisteredClaimNames.Sub, loginData.email),
                new Claim(JwtRegisteredClaimNames.Jti, Convert.ToString(userId))
            };
            var token = new JwtSecurityToken(issuer: _configuration["JwtSettings:Issuer"], audience: _configuration["JwtSettings:Audience"], claims: claims, expires: DateTime.Now.AddMinutes(tokenLifespan), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string IssueRefreshToken(LoginDto loginData)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JwtSettings:RefreshKey"]));
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
            int userId = _dbContext.Users.First(u => u.Email == loginData.email).Id;
            double tokenLifeSpan = GetRefreshTokenLifespan(loginData.rememberUser);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loginData.email)
            };
            var token = new JwtSecurityToken(issuer: _configuration["JwtSettings:Issuer"], audience: _configuration["JwtSettings:Audience"], claims: claims, expires: DateTime.Now.AddDays(tokenLifeSpan), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool RevokeRefreshToken(string refreshToken)
        {
            try
            {
                _dbContext.RevokedRefreshTokens.Add(new RevokedRefreshToken { Token = refreshToken });
                return true;
            }

            catch (Exception e)
            {
                _logger.LogCritical($"Could not invalidate refresh token {refreshToken}", e);
                return false;
            }
        }

        public bool RevokeAccessToken(string accessToken)
        {
            try
            {
                _dbContext.RevokedAccessTokens.Add(new RevokedAccessToken { Token = accessToken });
                return true;
            }

            catch (Exception e)
            {
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

    }
}



