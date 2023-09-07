using System;
namespace VerbTrainerAuth.Application.RefreshUserAccess
{
	public interface IRefreshUserAccessHandler
	{
        public Task<string> IssueNewAccessToken(string refreshToken, string oldAccessToken);
    }
}

