using System;
using Microsoft.AspNetCore.Mvc;
using VerbTrainer.DTOs;
using VerbTrainerAuth.Application.PasswordRecovery;
using VerbTrainerAuth.Application.User;
using VerbTrainerAuth.Domain.Entities;
using VerbTrainerAuth.Infrastructure.Data;
using VerbTrainerAuth.Infrastructure.Data.Models;
using VerbTrainerAuth.Infrastructure.Messaging.Producer;

namespace VerbTrainerAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PasswordRecoveryController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly IPasswordRecoveryServices _passwordRecovery;
        private readonly IUserServices _user;

        public PasswordRecoveryController(ILogger<RegisterController> logger,
                                          IPasswordRecoveryServices passwordServices,
                                          IUserServices userServices)
        {
            _logger = logger;
            _passwordRecovery = passwordServices;
            _user = userServices;
        }

        [HttpPost]
        public async Task<IActionResult> PasswordReminder([FromBody] string email)
        {
            // verify that email belongs to user
            // return user id
            // generate recovery token and save to the db;
            User? user = await _user.getUserInfoByEmail(email);
            if (user == null)
            {
                return StatusCode(400);
            }
            RecoveryTokenEntity tokenEntity = _passwordRecovery.CreateToken(user);
            await _passwordRecovery.SaveRecoveryToken(tokenEntity);
            // TODO: create mapper from entity to DTO
            return Ok("Email sent");
        }
    }
}

