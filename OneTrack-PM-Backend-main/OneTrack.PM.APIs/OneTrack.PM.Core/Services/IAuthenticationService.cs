using OneTrack.PM.Entities.DTOs.Security;
using OneTrack.PM.Entities.Models.DB;
using System.Security.Claims;

namespace OneTrack.PM.Core.Services
{
    public interface IAuthenticationService
    {
        Task<Tokens> CreateToken(SecUser user, string fireBaseToken);
        Task<Tokens> CreateRefreshToken(SecUser user, string fireBaseToken);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        bool IsTokenValid(string token);
        TokenClaims DecodeToken(string authorizationValue);
    }
}
