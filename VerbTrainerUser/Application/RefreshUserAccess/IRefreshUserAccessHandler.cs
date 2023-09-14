using System;
namespace VerbTrainerUser.Application.RefreshUserAccess
{
	public interface IRefreshUserAccessHandler
	{
        public Task<string> IssueNewAccessToken(string refreshToken, string oldAccessToken);
    }
}

