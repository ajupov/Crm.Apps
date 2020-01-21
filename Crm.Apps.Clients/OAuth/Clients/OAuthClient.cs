using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Http;
using Ajupov.Utils.All.String;
using Crm.Apps.Clients.OAuth.Mappers;
using Crm.Apps.Clients.OAuth.Models;
using Crm.Apps.Clients.OAuth.RequestParameters;
using Crm.Apps.Clients.OAuth.ResponseParameters;
using Microsoft.Extensions.Options;
using UriBuilder = Ajupov.Utils.All.Http.UriBuilder;

namespace Crm.Apps.Clients.OAuth.Clients
{
    public class OAuthClient : IOAuthClient
    {
        private const string ClientId = "http-client";
        private const string PasswordGrandType = "password";
        private const string RefreshTokenGrandType = "refresh_token";

        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public OAuthClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.OAuthHost, "Auth");
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Tokens> GetTokensAsync(string username, string password, CancellationToken ct = default)
        {
            var request = new TokenRequestParameter
            {
                grant_type = PasswordGrandType,
                client_id = ClientId,
                username = username,
                password = password
            };

            var response = await _httpClientFactory.PostAsync<TokenResponseParameter>(
                UriBuilder.Combine(_url, "Token"), request, ct);

            if (!response.error.IsEmpty())
            {
                throw new Exception(response.error);
            }

            return response.Map();
        }

        public async Task<Tokens> RefreshTokensAsync(string refreshToken, CancellationToken ct = default)
        {
            var request = new TokenRequestParameter
            {
                grant_type = RefreshTokenGrandType,
                client_id = ClientId,
                refresh_token = refreshToken
            };

            var response = await _httpClientFactory.PostAsync<TokenResponseParameter>(
                UriBuilder.Combine(_url, "Token"), request, ct);

            if (!response.error.IsEmpty())
            {
                throw new Exception(response.error);
            }

            return response.Map();
        }
    }
}