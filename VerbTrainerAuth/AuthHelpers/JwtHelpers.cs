using System;
namespace VerbTrainerAuth.AuthHelpers
{
	public class JwtHelpers
	{
        public static string? GetAccessTokenFromHeader(IHeaderDictionary headers)
        {
            string? authHeader = headers.Authorization;
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                string accessToken = authHeader.Substring("Bearer ".Length);
                return accessToken;
            }
            return null;
        }
    }
}

