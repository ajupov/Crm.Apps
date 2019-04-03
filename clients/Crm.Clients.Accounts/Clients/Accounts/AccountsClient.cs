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

        public Task<ICollection<Account>> GetListAsync(ICollection<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ICollection<Account>>($"{_settings.Host}/Api/Accounts/GetList",
                new {ids}, ct);
        }

        public Task<ICollection<Account>> GetPagedListAsync(bool? isLocked = default, bool? isDeleted = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default,
            int limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ICollection<Account>>($"{_settings.Host}/Api/Accounts/GetPagedList",
                new {isLocked, isDeleted, minCreateDate, maxCreateDate, offset, limit, sortBy, orderBy}, ct);
        }

        public Task<Guid> CreateAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Accounts/Create", ct: ct);
        }

        public Task UpdateAsync(Account newAccount, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Accounts/Update", newAccount, ct);
        }

        public Task LockAsync(ICollection<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Accounts/Lock", ids, ct);
        }

        public Task UnlockAsync(ICollection<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Accounts/Unlock", ids, ct);
        }

        public Task DeleteAsync(ICollection<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Accounts/Delete", ids, ct);
        }

        public Task RestoreAsync(ICollection<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Accounts/Restore", ids, ct);
        }
    }
}