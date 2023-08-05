using System;
using System.Security.Claims;
using VerbTrainerAuth.DTOs;

namespace VerbTrainerAuth.Services
{
	public interface IJWTService
	{
        string IssueAccessToken(IssueAccessTokenDto loginData);
        string IssueRefreshToken(LoginDto loginData);
        double GetRefreshTokenLifespan(bool rememberUser);
        bool RevokeAccessToken(string accessToken);
        bool RevokeRefreshToken(string refreshToken);
        bool ValidateToken(string token);
        IEnumerable<Claim> GetTokenPrincipal(string token);
        bool IsTokenExpired(string token);
        string? GetAccessTokenFromHeader(IHeaderDictionary headers);
    }
}

