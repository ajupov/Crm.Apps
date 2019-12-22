using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Auth.Models;
using Microsoft.AspNetCore.Authentication;

namespace Crm.Apps.Areas.Auth.Services
{
    public interface IAuthService
    {
        Task<Dictionary<string, string>> GetProvidersAsync();

        AuthenticationProperties GetExternalProperties(string ipAddress, string state, string callbackUri);

        Task<CallbackResponse> CallbackAsync(
            CallbackRequest request,
            ClaimsPrincipal user,
            string ipAddress,
            CancellationToken ct);

        void SignOut();
    }
}