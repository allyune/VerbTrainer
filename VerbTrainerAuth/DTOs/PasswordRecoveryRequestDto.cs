using System;
using VerbTrainerAuth.Domain.Entities;

namespace VerbTrainerAuth.DTOs
{
	public class PasswordResetRequestDto
	{
        public int UserId { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public int Status { get; private set; }
        public DateTime? LastLogin { get; private set; }
        public string? LastName { get; private set; }
        public string ResetLink { get; private set; }

        private PasswordResetRequestDto(
            int userId,
            string email,
            string firstName,
            string? lastName,
            int status,
            DateTime? lastLogin,
            string resetLink)
        {
            UserId = userId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Status = status;
            LastLogin = lastLogin;
            ResetLink = resetLink;
        }

        public static PasswordResetRequestDto CreateNew(
            int userId,
            string email,
            string firstName,
            int status,
            DateTime? lastLogin,
            string resetLink,
            string? lastName = null)
        {
            return new PasswordResetRequestDto(
                userId,
                email,
                firstName,
                lastName,
                status,
                lastLogin,
                resetLink);
        }
    }
}

