using System;
using System.Security.Claims;
using VerbTrainerUser.DTOs;

namespace VerbTrainerUser.Application.Services.JWT
{
	public interface IJWTService
	{
        Task<string> IssueAccessToken(IssueAccessTokenDto loginData);
        Task<string> IssueRefreshToken(LoginDto loginData);
        double GetRefreshTokenLifespan(bool rememberUser);
        Task<bool> RevokeAccessToken(string accessToken);
        Task<bool> RevokeRefreshToken(string refreshToken);
        bool ValidateToken(string token, out ClaimsPrincipal principal);
        Task<bool> IsAccessTokenBlacklisted(string token);
        Task<bool> IsRefreshTokenBlacklisted(string token);
    }
}

