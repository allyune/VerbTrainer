using System;
using VerbTrainer.DTOs;

namespace VerbTrainerAuth.Services
{
	public interface IJWTService
	{
        string IssueAccessToken(LoginDto loginData);
        string IssueRefreshToken(LoginDto loginData);
        double GetRefreshTokenLifespan(bool rememberUser);
        bool RevokeAccessToken(string accessToken);
        bool RevokeRefreshToken(string refreshToken);
    }
}

