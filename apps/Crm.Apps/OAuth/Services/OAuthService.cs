using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.OAuth.Controllers;
using Crm.Apps.OAuth.Models;
using Crm.Apps.OAuth.Storages;
using Crm.Clients.Identities.Clients;
using Crm.Clients.Identities.Models;
using Crm.Clients.Users.Clients;
using Crm.Utils.Http;

namespace Crm.Apps.OAuth.Services
{
    public class OAuthService : IOAuthService
    {
        private readonly IUsersClient _usersClient;
        private readonly IIdentitiesClient _identitiesClient;

        public OAuthService(
            IUsersClient usersClient,
            IIdentitiesClient identitiesClient)
        {
            _usersClient = usersClient;
            _identitiesClient = identitiesClient;
        }

        public async Task<PostAuthorizeResponse> AuthorizeAsync(
            PostAuthorizeRequest request,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            var identityTypes = IdentityTypeExtensions.TypesWithPassword;
            var identity = await _identitiesClient.GetByKeyAndTypesAsync(request.Key, identityTypes, ct);
            if (identity == null)
            {
                return new PostAuthorizeResponse(AuthenticationResponseError.InvalidCredentials);
            }

            var user = await _usersClient.GetAsync(identity.UserId, ct);
            if (user == null)
            {
                return new PostAuthorizeResponse(AuthenticationResponseError.InvalidCredentials);
            }

            var isPasswordCorrect = await _identitiesClient.IsPasswordCorrectAsync(request.Key, request.Password, ct);
            if (!isPasswordCorrect)
            {
                return new PostAuthorizeResponse(AuthenticationResponseError.InvalidCredentials);
            }

            var parameters = GetRedirectUrlParameters(request.ResponseType);

            var redirectUri = request.RedirectUri.AddParameters(parameters);

            return new PostAuthorizeResponse(redirectUri);
        }

        public Task<TokenResponse> GetTokenAsync(
            TokenRequest request,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            return Task.FromResult(new TokenResponse("", "", "", 0));
        }

        private static (string key, object value)[] GetRedirectUrlParameters(
            string responseType)
        {
            switch (responseType)
            {
                case AuthorizeResponseType.Code:
                {
                    return new (string key, object value)[]
                    {
                        ("code", "code")
                    };
                }

                case AuthorizeResponseType.Token:
                {
                    return new (string key, object value)[]
                    {
                        ("token_type", "bearer"),
                        ("access_token", "accessToken"),
                        ("refresh_token", "refreshToken"),
                        ("expires_in", TimeSpan.FromDays(1).TotalSeconds)
                    };
                }

                default:
                    throw new ArgumentOutOfRangeException(responseType);
            }
        }
    }
}