using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VerbTrainerAuth.Models.Domain;
using VerbTrainerAuth.Data;
using VerbTrainerAuth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using VerbTrainerAuth.DTOs;
using System.Security.Claims;
using System.Security.Principal;
using VerbTrainerAuth.AuthHelpers;

namespace VerbTrainerAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly VerbTrainerAuthDbContext _dbContext;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IJWTService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthController(ILogger<AuthController> logger, VerbTrainerAuthDbContext dbContext,
                              IPasswordHashService passwordHashService, IJWTService jWTService,
                              IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _passwordHashService = passwordHashService;
            _jwtService = jWTService;
            _configuration = configuration;
            //_httpContextAccessor = httpContextAccessor;
        }


        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] LoginDto data)
        {
            string email = data.email;
            string password = data.password;
            bool remeberUser = data.rememberUser;
            User? userInfo = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (userInfo == null)
            {
                return NotFound("Wrong email or password");
            };

            string savedPasswordHash = userInfo.Password;
            string savedSalt = userInfo.Salt;
            bool passwordValid = _passwordHashService.VerifyPasswordHash(password, savedPasswordHash, savedSalt);
            if (passwordValid)
            {
                //rovoke tokens if exist
                string? authHeader = Request.Headers.Authorization;
                _ = Request.Cookies.TryGetValue("RefreshToken", out var oldRefreshToken);

                if (authHeader != null)
                {
                    string oldAccessToken = authHeader.Substring("Bearer ".Length);
                    _jwtService.RevokeAccessToken(oldAccessToken);
                }

                if (oldRefreshToken != null)
                {
                    _jwtService.RevokeRefreshToken(oldRefreshToken);
                }

                string accessToken = _jwtService.IssueAccessToken(new IssueAccessTokenDto { Email = data.email});
                string refreshToken = _jwtService.IssueRefreshToken(data);
                double refreshTokenLifespan = _jwtService.GetRefreshTokenLifespan(data.rememberUser);

                CookieOptions cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.Now.AddDays(refreshTokenLifespan),
                    Secure = false
            };

                if (Request.IsHttps)
                {
                    cookieOptions.Secure = true;
                }

                Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
                return Ok(accessToken);
            }

            return NotFound("Wrong email or password");
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken()
        {
            string? oldAccessToken = JwtHelpers.GetAccessTokenFromHeader(Request.Headers);
            if (oldAccessToken != null && Request.Cookies.TryGetValue("RefreshToken", out string refreshToken))
            {
                if (!_jwtService.IsRefreshTokenBlacklisted(refreshToken) && _jwtService.ValidateToken(refreshToken))
                {
                    IEnumerable<Claim> tokenClaims = _jwtService.GetTokenPrincipal(oldAccessToken);
                    string emailClaimValue = tokenClaims.SingleOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
                    string newAccessToken = _jwtService.IssueAccessToken(new IssueAccessTokenDto { Email = emailClaimValue });
                    bool oldTokenRevoked = _jwtService.RevokeAccessToken(oldAccessToken);
                    if (!oldTokenRevoked && !_jwtService.IsTokenExpired(oldAccessToken))
                    {
                        return Ok(oldAccessToken);
                    }
                    else if (!oldTokenRevoked)
                    {
                        return Ok(newAccessToken);
                    }
                    return Ok(newAccessToken);
                }
                _jwtService.RevokeRefreshToken(refreshToken);
                _jwtService.RevokeAccessToken(oldAccessToken);
                return Unauthorized("Refresh token not valid");
            }
            return BadRequest("One of tokens is not provided");

        }
     }
}

