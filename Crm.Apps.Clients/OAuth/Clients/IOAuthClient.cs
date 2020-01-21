using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.OAuth.Models;

namespace Crm.Apps.Clients.OAuth.Clients
{
    public interface IOAuthClient
    {
        Task<Tokens> GetTokensAsync(string username, string password, CancellationToken ct = default);

        Task<Tokens> RefreshTokensAsync(string refreshToken, CancellationToken ct = default);
    }
}