using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.OAuth.Models;

namespace Crm.Apps.v1.Clients.OAuth.Clients
{
    public interface IOAuthClient
    {
        Task<Tokens> GetTokensAsync(string username, string password, string accessToken, CancellationToken ct = default);

        Task<Tokens> RefreshTokensAsync(string refreshToken, string accessToken, CancellationToken ct = default);
    }
}