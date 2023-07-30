using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VerbTrainer.Data;
using VerbTrainer.DTOs;
using VerbTrainer.Models.Domain;
using VerbTrainer.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VerbTrainer.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly ILogger<DeckController> _logger;
        private readonly VerbTrainerDbContext _dbContext;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IConfiguration _configuration;

        public AuthController(ILogger<DeckController> logger, VerbTrainerDbContext dbContext, IPasswordHashService passwordHashService, IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _passwordHashService = passwordHashService;
            _configuration = configuration;
        }

        private string IssueJwt(LoginDto loginData)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"])); // NOTE: SAME KEY AS USED IN Startup.cs FILE
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
            int userId = _dbContext.Users.First(u => u.Email == loginData.email).Id;
            int TokenLifeSpan;
            if (loginData.rememberUser)
            {
                // remeebr user for 5 days
                TokenLifeSpan = 7200;
            }
            else
            {
                TokenLifeSpan = 1440;
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loginData.email),
                new Claim(JwtRegisteredClaimNames.Sub, loginData.email),
                new Claim(JwtRegisteredClaimNames.Jti, Convert.ToString(userId))
            };
            var token = new JwtSecurityToken(issuer: _configuration["JwtSettings:Issuer"], audience: _configuration["JwtSettings:Audience"], claims: claims, expires: DateTime.Now.AddMinutes(TokenLifeSpan), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] RegisterDto data)
        {
            string email = data.email;
            string password = data.password;
            string passwordHash = _passwordHashService.HashPassword(password, out var saltString);
            _dbContext.Users.Add(new User { Email = email, Password = passwordHash, Salt = saltString });
            _dbContext.SaveChanges();
            return Ok("User added");
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] LoginDto data)
        {
            string email = data.email;
            string password = data.password;
            bool remeberUser = data.rememberUser;
            User? userInfo = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            Console.WriteLine(userInfo);
            if (userInfo == null)
            {
                return NotFound("Wrong email or password");
            };

            string savedPasswordHash = userInfo.Password;
            string savedSalt = userInfo.Salt;
            bool passwordValid = _passwordHashService.VerifyPasswordHash(password, savedPasswordHash, savedSalt);
            Console.WriteLine(passwordValid);
            if (passwordValid)
            {
                return Ok(new { token = IssueJwt(data) });
            }

            return NotFound("Wrong email or password");
        }

    }
}

