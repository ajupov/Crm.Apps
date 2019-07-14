using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;
using Crm.Clients.Accounts.Parameters;
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

        public AccountsClient(
            IOptions<AccountsClientSettings> options,
            IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Api/Accounts");
            _httpClientFactory = httpClientFactory;
        }

        public Task<Dictionary<AccountType, string>> GetTypesAsync(
            CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Dictionary<AccountType, string>>($"{_url}/GetTypes", ct: ct);
        }

        public Task<Account> GetAsync(
            Guid id,
            CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Account>($"{_url}/Get", new {id}, ct);
        }

        public Task<Account[]> GetListAsync(
            IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Account[]>($"{_url}/GetList", ids, ct);
        }

        public Task<Account[]> GetPagedListAsync(
            bool? isLocked = default,
            bool? isDeleted = default,
            DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default,
            AccountType[] types = default,
            int offset = default,
            int limit = 10,
            string sortBy = "CreateDateTime",
            string orderBy = "desc",
            CancellationToken ct = default)
        {
            var parameter = new AccountGetPagedListParameter(
                isLocked, isDeleted, minCreateDate, maxCreateDate, types, offset, limit, sortBy, orderBy);

            return _httpClientFactory.PostAsync<Account[]>($"{_url}/GetPagedList", parameter, ct);
        }

        public Task<Guid> CreateAsync(
            Account account,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_url}/Create", account, ct);
        }

        public Task UpdateAsync(
            Account account,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/Update", account, ct);
        }

        public Task LockAsync(
            IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/Lock", ids, ct);
        }

        public Task UnlockAsync(
            IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/Unlock", ids, ct);
        }

        public Task DeleteAsync(
            IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/Delete", ids, ct);
        }

        public Task RestoreAsync(
            IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/Restore", ids, ct);
        }
    }
}