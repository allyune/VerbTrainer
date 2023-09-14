using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VerbTrainerUser.DTOs;
using VerbTrainerUser.Application.Services.JWT;
using VerbTrainerUser.Application.UserLogin;
using VerbTrainerUser.Application.RefreshUserAccess;

namespace VerbTrainerUser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserLoginHandler _loginHandler;
        private readonly IRefreshUserAccessHandler _refreshHandler;
        private readonly IJWTService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthController(
            ILogger<AuthController> logger,
            IUserLoginHandler loginHansler,
            IRefreshUserAccessHandler refreshUserAccessHandler,
            IJWTService jWTService,
            IConfiguration configuration)
        {
            _logger = logger;
            _loginHandler = loginHansler;
            _refreshHandler = refreshUserAccessHandler;
            _jwtService = jWTService;
            _configuration = configuration;
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto data)
        {
            string email = data.email;
            string password = data.password;
            string? authHeader = Request.Headers.Authorization;
            IRequestCookieCollection cookies = Request.Cookies;
            bool IsLoginSuccessful = await _loginHandler.UserLogin(
                email, password, authHeader, cookies);

            if (!IsLoginSuccessful)
            {
                return Unauthorized("User or password are invalid");
            }
            string accessToken = await _jwtService.IssueAccessToken(new IssueAccessTokenDto(data.email));
            string refreshToken = await _jwtService.IssueRefreshToken(data);
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
            

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            string? authHeader = Request.Headers.Authorization;
            if (authHeader is null)
            {
                return BadRequest("Authorization header is not provided");
            }

            if (authHeader.StartsWith("Bearer "))
            {
                string oldAccessToken = authHeader.Substring("Bearer ".Length);

                if (oldAccessToken != null && Request.Cookies.TryGetValue("RefreshToken", out string? refreshToken))
                {
                    try
                    {
                        string newAccessToken = await _refreshHandler.IssueNewAccessToken(refreshToken, oldAccessToken);
                        return Ok(newAccessToken);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("Could not validate refresh token " + e.Message);
                        await _jwtService.RevokeRefreshToken(refreshToken);
                        await _jwtService.RevokeAccessToken(oldAccessToken);
                        return Unauthorized("Refresh token not valid");
                    }
                }
            }
            return BadRequest("One of tokens is not provided");

        }
     }
}

