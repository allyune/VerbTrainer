using System;
using System.Security.Claims;
using System.Security.Principal;
using VerbTrainerAuth.Application.Services.JWT;
using VerbTrainerAuth.DTOs;

namespace VerbTrainerAuth.Application.RefreshUserAccess
{
    public class RefreshUserAccessHandler : IRefreshUserAccessHandler
    {
        public readonly IJWTService _jwtService;
        public readonly ILogger<RefreshUserAccessHandler> _logger;

        public RefreshUserAccessHandler(
            IJWTService jwtService,
            ILogger<RefreshUserAccessHandler> logger)
        {
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<string> IssueNewAccessToken(
            string refreshToken, string oldAccessToken)
        {

            bool isTokenBlacklisted = await _jwtService.IsRefreshTokenBlacklisted(refreshToken);
            if (isTokenBlacklisted)
            {
                throw new Exception("Refresh token is blacklisted");
            }
                    
            bool isTokenValid = _jwtService.ValidateToken(
                refreshToken, out ClaimsPrincipal principal);
            
            if (!isTokenValid)
            {
                throw new Exception("Refresh token is not valid");
            }

            // Presense of email claim is checked during token validation
            IEnumerable<Claim> tokenClaims = principal.Claims;
            string? emailClaimValue = tokenClaims.SingleOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
            string newAccessToken = await _jwtService.IssueAccessToken(new IssueAccessTokenDto(emailClaimValue));
            bool oldTokenRevoked = await _jwtService.RevokeAccessToken(oldAccessToken);
            if (!oldTokenRevoked)
            {
                return oldAccessToken;
            }

            return newAccessToken;
        }
    }
}