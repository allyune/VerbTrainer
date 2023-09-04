using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using VerbTrainerEmail.Application.User;
using VerbTrainerEmail.Domain.Entities.User;
using System.Net.Http.Json;

namespace VerbTrainerEmail.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IUserService _userService;

        public EmailController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("finduser")]
        public async Task<IActionResult> GetUser([FromBody] int userId)
        {
            User user = await _userService.GetUserById(userId);
            return Ok(user.FirstName);
        }
    }

}
