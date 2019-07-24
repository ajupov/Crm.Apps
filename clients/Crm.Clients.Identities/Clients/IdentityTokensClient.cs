using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Identities.Models;
using Crm.Clients.Identities.Parameters;
using Crm.Clients.Identities.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;
using UriBuilder = Crm.Utils.Http.UriBuilder;

namespace Crm.Clients.Identities.Clients
{
    public class IdentityTokensClient : IIdentityTokensClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public IdentityTokensClient(
            IOptions<IdentitiesClientSettings> options,
            IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Api/Identities/Tokens");
            _httpClientFactory = httpClientFactory;
        }

        public Task<IdentityToken> GetAsync(
            Guid identityId,
            string value,
            CancellationToken ct = default)
        {
            var parameter = new IdentityTokenGetParameter(identityId, value);

            return _httpClientFactory.GetAsync<IdentityToken>($"{_url}/Get", parameter, ct);
        }

        public Task<Guid> CreateAsync(
            IdentityToken token,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_url}/Create", token, ct);
        }

        public Task SetIsUsedAsync(
            Guid id,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/SetIsUsed", id, ct);
        }
    }
}