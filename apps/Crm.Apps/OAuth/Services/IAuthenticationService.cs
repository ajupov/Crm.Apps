using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.OAuth.Models;
using Microsoft.AspNetCore.Authentication;

namespace Crm.Apps.OAuth.Services
{
    public interface IAuthenticationService
    {
        Task<Dictionary<string, string>> GetProvidersAsync();

        AuthenticationProperties GetExternalProperties(
            string state,
            string ipAddress,
            string callbackUri);

        Task<CallbackResponse> CallbackAsync(
            CallbackRequest request,
            ClaimsPrincipal user,
            string ipAddress,
            string userAgent,
            CancellationToken ct);

        void SignOut();
    }
}