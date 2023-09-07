using System;
using Microsoft.AspNetCore.Mvc;
using VerbTrainerAuth.Application.ResetPassword;

namespace VerbTrainerAuth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResetPasswordController : ControllerBase
    {
        private readonly ILogger<ResetPasswordController> _logger;
        private readonly IResetPasswordHandler _passwordResetHandler;

        public ResetPasswordController(ILogger<ResetPasswordController> logger,
                                          IResetPasswordHandler resetPassword)
                                          
        {
            _logger = logger;
            _passwordResetHandler = resetPassword;
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] string email)
        {
            try
            {
               await _passwordResetHandler.ResetPassword(email);
            }

            catch (Exception e)
            {
                _logger.LogInformation(
                    $"Could not send a password reset email for user {email}: {e.Message}");
            }
            // always returning Ok for security purposes;
            // not revealing to the user that email is not found;
            return Ok("Email sent");
        }
    }
}

