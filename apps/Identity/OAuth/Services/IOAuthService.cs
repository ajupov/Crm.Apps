using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Identity.OAuth.Models.Authorize;
using Identity.OAuth.Models.Tokens;

namespace Identity.OAuth.Services
{
    public interface IOAuthService
    {
        bool IsAuthorized(ClaimsPrincipal claimsPrincipal);

        Task<PostAuthorizeResponse> AuthorizeAsync(
            PostAuthorizeRequest request,
            string userAgent,
            string ipAddress,
            CancellationToken ct);

        Task<TokenResponse> GetTokenAsync(
            TokenRequest request,
            ClaimsPrincipal claimsPrincipal,
            string userAgent,
            string ipAddress,
            CancellationToken ct);
    }
}