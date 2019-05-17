using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;
using Crm.Clients.Accounts.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Accounts.Clients.AccountChanges
{
    public class AccountChangesClient : IAccountChangesClient
    {
        private readonly AccountsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountChangesClient(IHttpClientFactory httpClientFactory, IOptions<AccountsClientSettings> options)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public Task<List<AccountChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? accountId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default,
            int limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<AccountChange>>(
                $"{_settings.Host}/Api/Accounts/Changes/GetPagedList", new
                {
                    changerUserId, accountId, minCreateDate, maxCreateDate, offset, limit, sortBy, orderBy
                }, ct);
        }
    }
}