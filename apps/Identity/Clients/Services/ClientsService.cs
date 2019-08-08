using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Utils.Sorting;
using Identity.Clients.Models;
using Identity.Clients.Parameters;
using Identity.Clients.Storages;
using Microsoft.EntityFrameworkCore;

namespace Identity.Clients.Services
{
    public class ClientsService : IClientsService
    {
        private readonly ClientsStorage _clientsStorage;

        public ClientsService(ClientsStorage clientsStorage)
        {
            _clientsStorage = clientsStorage;
        }

        public Task<Client> GetAsync(Guid id, CancellationToken ct)
        {
            return _clientsStorage.Clients
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<Client> GetByClientIdAsync(string clientId, CancellationToken ct)
        {
            return _clientsStorage.Clients
                .FirstOrDefaultAsync(x => x.ClientId == clientId, ct);
        }

        public Task<Client[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _clientsStorage.Clients
                .Where(x => ids.Contains(x.Id))
                .ToArrayAsync(ct);
        }

        public Task<Client[]> GetPagedListAsync(ClientGetPagedListParameter parameter, CancellationToken ct)
        {
            return _clientsStorage.Clients
                .Where(x =>
                    (!parameter.IsLocked.HasValue || x.IsLocked == parameter.IsLocked) &&
                    (!parameter.IsDeleted.HasValue || x.IsDeleted == parameter.IsDeleted) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToArrayAsync(ct);
        }

        public async Task<Guid> CreateAsync(Client client, CancellationToken ct)
        {
            var newClient = new Client(client.ClientId, client.ClientSecret, client.RedirectUriPattern, client.IsLocked,
                client.IsDeleted);

            var entry = await _clientsStorage.AddAsync(newClient, ct);
            await _clientsStorage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public Task UpdateAsync(Client oldClient, Client client, CancellationToken ct)
        {
            oldClient.ClientId = client.ClientId;
            oldClient.ClientSecret = client.ClientSecret;
            oldClient.RedirectUriPattern = client.RedirectUriPattern;
            oldClient.IsLocked = client.IsLocked;
            oldClient.IsDeleted = client.IsDeleted;
            oldClient.Scopes = client.Scopes
                .Select(s => new ClientScope(s.ClientId, s.Value))
                .ToArray();

            _clientsStorage.Update(oldClient);

            return _clientsStorage.SaveChangesAsync(ct);
        }

        public async Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _clientsStorage.Clients
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => x.IsLocked = true, ct);

            await _clientsStorage.SaveChangesAsync(ct);
        }

        public async Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _clientsStorage.Clients
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => x.IsLocked = false, ct);

            await _clientsStorage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _clientsStorage.Clients
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => x.IsDeleted = true, ct);

            await _clientsStorage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _clientsStorage.Clients
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => x.IsDeleted = false, ct);

            await _clientsStorage.SaveChangesAsync(ct);
            ;
        }
    }
}