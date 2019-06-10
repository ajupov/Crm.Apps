using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Identities.Models;
using Crm.Clients.Identities.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Identities.Clients
{
    public class IdentityChangesClient : IIdentityChangesClient
    {
        private readonly IdentitiesClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public IdentityChangesClient(IHttpClientFactory httpClientFactory, IOptions<IdentitiesClientSettings> options)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public Task<List<IdentityChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? identityId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default,
            int limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default)
        {
            var parameter = new
            {
                ChangerUserId = changerUserId,
                IdentityId = identityId,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<IdentityChange>>(
                $"{_settings.Host}/Api/Identities/Changes/GetPagedList", parameter, ct);
        }
    }
}