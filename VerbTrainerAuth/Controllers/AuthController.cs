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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(ILogger<AuthController> logger, VerbTrainerAuthDbContext dbContext,
                              IPasswordHashService passwordHashService, IJWTService jWTService,
                              IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _dbContext = dbContext;
            _passwordHashService = passwordHashService;
            _jwtService = jWTService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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

                HttpContext context = _httpContextAccessor.HttpContext;

                if (Request.IsHttps)
                {
                    cookieOptions.Secure = true;
                }

                context.Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
                return Ok(accessToken);
            }

            return NotFound("Wrong email or password");
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] AccessTokenDto AccessTokenPayload)
        {
            HttpContext context = _httpContextAccessor.HttpContext;
            if (context.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
            {
                if (_jwtService.ValidateToken(refreshToken))
                {
                    string oldAccessToken = AccessTokenPayload.AccessToken;
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
                _jwtService.RevokeAccessToken(AccessTokenPayload.AccessToken);
                return Unauthorized("Refresh token not valid");
            }
            _jwtService.RevokeAccessToken(AccessTokenPayload.AccessToken);
            return BadRequest("No Refresh token provided");

        }
     }
}

