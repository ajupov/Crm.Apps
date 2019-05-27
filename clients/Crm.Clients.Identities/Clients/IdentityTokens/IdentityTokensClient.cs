using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Identities.Models;
using Crm.Clients.Identities.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Identities.Clients.IdentityTokens
{
    public class IdentityTokensClient : IIdentityTokensClient
    {
        private readonly IdentitiesClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public IdentityTokensClient(IOptions<IdentitiesClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<IdentityToken> GetAsync(Guid identityId, string value, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<IdentityToken>($"{_settings.Host}/Api/Identities/Tokens/Get",
                new {identityId, value}, ct);
        }

        public Task<Guid> CreateAsync(IdentityToken token, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Identities/Tokens/Create", token, ct);
        }

        public Task SetIsUsedAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Identities/Tokens/SetIsUsed", id, ct);
        }
    }
}