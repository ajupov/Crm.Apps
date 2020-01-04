using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;
using Crm.Clients.Accounts.RequestParameters;
using Crm.Clients.Accounts.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;
using UriBuilder = Crm.Utils.Http.UriBuilder;

namespace Crm.Clients.Accounts.Clients
{
    public class AccountsClient : IAccountsClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountsClient(IOptions<AccountsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Api/Accounts");
            _httpClientFactory = httpClientFactory;
        }

        public Task<Dictionary<string, AccountType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Dictionary<string, AccountType>>($"{_url}/GetTypes", ct: ct);
        }

        public Task<Account> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Account>($"{_url}/Get", new {id}, ct);
        }

        public Task<Account[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Account[]>($"{_url}/GetList", ids, ct);
        }

        public Task<Account[]> GetPagedListAsync(AccountGetPagedListParameter request, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Account[]>($"{_url}/GetPagedList", request, ct);
        }

        public Task<Guid> CreateAsync(AccountCreateRequest request, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_url}/Create", request, ct);
        }

        public Task UpdateAsync(AccountUpdateRequest request, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/Update", request, ct);
        }

        public Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/Lock", ids, ct);
        }

        public Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/Unlock", ids, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/Restore", ids, ct);
        }
    }
}