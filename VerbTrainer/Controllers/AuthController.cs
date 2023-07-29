using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public AuthController(ILogger<DeckController> logger, VerbTrainerDbContext dbContext, IPasswordHashService passwordHashService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _passwordHashService = passwordHashService;
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

    }
}

