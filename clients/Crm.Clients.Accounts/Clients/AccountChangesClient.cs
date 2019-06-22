using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;
using Crm.Clients.Accounts.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Accounts.Clients
{
    public class AccountChangesClient : IAccountChangesClient
    {
        private readonly AccountsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountChangesClient(IOptions<AccountsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<AccountChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? accountId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default,
            int limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default)
        {
            var parameter = new
            {
                ChangerUserId = changerUserId,
                AccountId = accountId,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<AccountChange>>(
                $"{_settings.Host}/Api/Accounts/Changes/GetPagedList", parameter, ct);
        }
    }
}