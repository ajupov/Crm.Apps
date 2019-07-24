using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.OAuth.Controllers;
using Crm.Apps.OAuth.Models;

namespace Crm.Apps.OAuth.Services
{
    public interface IOAuthService
    {
        Task<PostAuthorizeResponse> AuthorizeAsync(
            PostAuthorizeRequest request,
            string userAgent,
            string ipAddress,
            CancellationToken ct);

        Task<TokenResponse> GetTokenAsync(
            TokenRequest request,
            string userAgent,
            string ipAddress,
            CancellationToken ct);
    }
}