using System;
namespace VerbTrainerEmail.Application.Contracts.DTOs
{
    public class ResetPasswordRequestDto : BaseDto
    {
        public string ResetLink { get; private set; }
    }

}