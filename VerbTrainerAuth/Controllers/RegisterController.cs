using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VerbTrainer.DTOs;
using VerbTrainerAuth.Models.Domain;
using VerbTrainerAuth.Data;
using VerbTrainerAuth.Services;

namespace VerbTrainerAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly VerbTrainerAuthDbContext _dbContext;
        private readonly IPasswordHashService _passwordHashService;

        public RegisterController(ILogger<RegisterController> logger, VerbTrainerAuthDbContext dbContext, IPasswordHashService passwordHashService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _passwordHashService = passwordHashService;
        }

        [HttpPost]
        public IActionResult RegisterUser([FromBody] RegisterDto data)
        {
            string email = data.email;
            string password = data.password;
            User? checkExists = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (checkExists != null)
            {
                return BadRequest($"User with email {email} already exists");
            }
            string passwordHash = _passwordHashService.HashPassword(password, out var saltString);
            _dbContext.Users.Add(new User { Email = email, Password = passwordHash, Salt = saltString });
            _dbContext.SaveChanges();
            return Ok("User added");
        }
    }
}

