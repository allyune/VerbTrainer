using System;
using VerbTrainerAuth.Application.Services.JWT;
using VerbTrainerAuth.Application.Services.User;
using VerbTrainerAuth.Domain.Entities;
using VerbTrainerAuth.Infrastructure.Data.Models;

namespace VerbTrainerAuth.Application.UserLogin
{
    internal sealed class UserLoginHandler : IUserLoginHandler
    {
        private readonly IUserService _userService;
        private readonly IJWTService _jwtService;
        private readonly ILogger<UserLoginHandler> _logger;

        public UserLoginHandler(
            IUserService userService,
            IJWTService jwtService,
            ILogger<UserLoginHandler> logger)
        {
            _userService = userService;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<bool> UserLogin
            (string email,
            string password,
            string? authHeader,
            IRequestCookieCollection cookies)
        {
            bool userExists = await _userService.CheckUserExists(email);

            if (!userExists)
            {
                return false;
            };

            UserEntity user = await _userService.getUserInfoByEmail(email);
            bool isPasswordValid = user.VerifyPassword(password);
            if (!isPasswordValid)
            {
                return false;
            }

            if (authHeader is not null)
            {
                string oldAccessToken = authHeader.Substring("Bearer ".Length);
                await _jwtService.RevokeAccessToken(oldAccessToken);
            }

            _ = cookies.TryGetValue("RefreshToken", out var oldRefreshToken);

            if (oldRefreshToken is not null)
            {
                await _jwtService.RevokeRefreshToken(oldRefreshToken);
            }

            return true;

        }
    }
}

