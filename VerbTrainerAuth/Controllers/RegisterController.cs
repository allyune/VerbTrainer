using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VerbTrainer.DTOs;
using VerbTrainerUser.Application.Exceptions;
using VerbTrainerUser.Application.UserRegister;

namespace VerbTrainerUser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterUserHandler _registerHandler;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(
            IRegisterUserHandler registerHandler,
            ILogger<RegisterController> logger)
        {
            _registerHandler = registerHandler;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto data)
        {
            string email = data.email;
            string password = data.password;
            string firstName = data.firstName;
            Console.WriteLine();
            try
            {
                bool isUserAdded = await _registerHandler.RegisterUser(
                    email, password, firstName);
                return Ok("User added");
            }

            catch (Exception e)
            {
                switch (e)
                {
                    case UserAlreadyExistsException ex:
                        return BadRequest("User already exists");
                    default:
                        _logger.LogCritical(e.Message);
                        return BadRequest("Error while registering a user");
                }
            }
        }
    }
}

