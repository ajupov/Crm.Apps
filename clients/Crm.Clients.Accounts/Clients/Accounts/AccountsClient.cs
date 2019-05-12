using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;
using Crm.Clients.Accounts.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Accounts.Clients.Accounts
{
    public class AccountsClient : IAccountsClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AccountsClientSettings _settings;

        public AccountsClient(IOptions<AccountsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public Task<Account> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Account>($"{_settings.Host}/Api/Accounts/Get", new {id}, ct);
        }

        public Task<List<Account>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<Account>>($"{_settings.Host}/Api/Accounts/GetList",
                new {ids}, ct);
        }

        public Task<List<Account>> GetPagedListAsync(bool? isLocked = default, bool? isDeleted = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default,
            int limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<Account>>($"{_settings.Host}/Api/Accounts/GetPagedList",
                new {isLocked, isDeleted, minCreateDate, maxCreateDate, offset, limit, sortBy, orderBy}, ct);
        }

        public Task<Guid> CreateAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Accounts/Create", ct: ct);
        }

        public Task UpdateAsync(Account account, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Accounts/Update", account, ct);
        }

        public Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Accounts/Lock", ids, ct);
        }

        public Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Accounts/Unlock", ids, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Accounts/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Accounts/Restore", ids, ct);
        }
    }
}