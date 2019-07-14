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
    public class AccountChangesClient : IAccountChangesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountChangesClient(
            IOptions<AccountsClientSettings> options,
            IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Api/Accounts/Changes");
            _httpClientFactory = httpClientFactory;
        }

        public Task<AccountChange[]> GetPagedListAsync(
            Guid accountId,
            Guid? changerUserId = default,
            DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default,
            int offset = default,
            int limit = 10,
            string sortBy = default,
            string orderBy = default,
            CancellationToken ct = default)
        {
            var parameter = new AccountChangeGetPagedListParameter(
                accountId, changerUserId, minCreateDate, maxCreateDate, offset, limit, sortBy, orderBy);

            return _httpClientFactory.PostAsync<AccountChange[]>($"{_url}/GetPagedList", parameter, ct);
        }
    }
}