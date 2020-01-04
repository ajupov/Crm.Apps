using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Http;
using Crm.Apps.Clients.Accounts.Models;
using Crm.Apps.Clients.Accounts.RequestParameters;
using Microsoft.Extensions.Options;
using UriBuilder = Ajupov.Utils.All.Http.UriBuilder;

namespace Crm.Apps.Clients.Accounts.Clients
{
    public class AccountsClient : IAccountsClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountsClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Accounts");
            _httpClientFactory = httpClientFactory;
        }

        public Task<Dictionary<string, AccountType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Dictionary<string, AccountType>>(
                UriBuilder.Combine(_url, "GetTypes"), ct: ct);
        }

        public Task<Account> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Account>(UriBuilder.Combine(_url, "Get"), new {id}, ct);
        }

        public Task<List<Account>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<Account>>(UriBuilder.Combine(_url, "GetList"), ids, ct);
        }

        public Task<List<Account>> GetPagedListAsync(
            AccountGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<Account>>(UriBuilder.Combine(_url, "GetPagedList"), request, ct);
        }

        public Task<Guid> CreateAsync(Account account, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>(UriBuilder.Combine(_url, "Create"), account, ct);
        }

        public Task UpdateAsync(Account account, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync(UriBuilder.Combine(_url, "Update"), account, ct);
        }

        public Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync(UriBuilder.Combine(_url, "Lock"), ids, ct);
        }

        public Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync(UriBuilder.Combine(_url, "Unlock"), ids, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync(UriBuilder.Combine(_url, "Delete"), ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync(UriBuilder.Combine(_url, "Restore"), ids, ct);
        }
    }
}