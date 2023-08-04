using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VerbTrainer.DTOs;
using VerbTrainerAuth.Models.Domain;
using VerbTrainerAuth.Data;
using VerbTrainerAuth.Services;
using Microsoft.AspNetCore.Http;


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
                string accessToken = _jwtService.IssueAccessToken(data);
                string refreshToken = _jwtService.IssueRefreshToken(data);
                double refreshTokenLifespan = _jwtService.GetRefreshTokenLifespan(data.rememberUser);

                CookieOptions cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.Now.AddDays(refreshTokenLifespan)
                };
         

                if (Request.IsHttps)
                {
                    cookieOptions.Secure = true;
                }

                HttpContext context = _httpContextAccessor.HttpContext;

                context.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
                return Ok(accessToken);
            }

            return NotFound("Wrong email or password");
        }
    }
}

