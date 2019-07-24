using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.OAuth.Models;
using Crm.Apps.OAuth.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.OAuth.Services
{
    public class OAuthClientsService
    {
        private readonly OAuthClientsStorage _storage;

        public OAuthClientsService(
            OAuthClientsStorage storage)
        {
            _storage = storage;
        }

        public Task<OAuthClient> GetAsync(
            int id,
            CancellationToken ct)
        {
            return _storage.Clients.FirstOrDefaultAsync(x => x.Id == id, ct);
        }
    }
}