using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Crm.Infrastructure.HotStorage.HotStorage;
using Crm.Utils.Generator;
using Crm.Utils.Http;
using Identity.Identities.Extensions;
using Identity.Identities.Models;
using Identity.Identities.Parameters;
using Identity.Identities.Services;
using Identity.OAuth.Models.Authorize;
using Identity.OAuth.Models.Tokens;
using Identity.OAuth.Options;
using Identity.Users.Models;
using Identity.Users.Services;
using Microsoft.IdentityModel.Tokens;

namespace Identity.OAuth.Services
{
    public class OAuthService : IOAuthService
    {
        private readonly IHotStorage _hotStorage;
        private readonly IUsersService _usersService;
        private readonly IIdentitiesService _identitiesService;
        private readonly IIdentityTokensService _identityTokensService;

        public OAuthService(
            IHotStorage hotStorage,
            IUsersService usersService,
            IIdentitiesService identitiesService,
            IIdentityTokensService identityTokensService)
        {
            _hotStorage = hotStorage;
            _usersService = usersService;
            _identitiesService = identitiesService;
            _identityTokensService = identityTokensService;
        }

        public bool IsAuthorized(ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            return claim != null;
        }

        public async Task<PostAuthorizeResponse> AuthorizeAsync(
            PostAuthorizeRequest request,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            var identityTypes = IdentityTypeExtensions.TypesWithPassword;
            var identity = await _identitiesService.GetByKeyAndTypesAsync(request.Key, identityTypes, ct);
            if (identity == null)
            {
                return new PostAuthorizeResponse(AuthorizeResponseError.InvalidCredentials);
            }

            var user = await _usersService.GetAsync(identity.UserId, ct);
            if (user == null)
            {
                return new PostAuthorizeResponse(AuthorizeResponseError.InvalidCredentials);
            }

            var isPasswordCorrect = _identitiesService.IsPasswordCorrect(identity, request.Password);
            if (!isPasswordCorrect)
            {
                return new PostAuthorizeResponse(AuthorizeResponseError.InvalidCredentials);
            }

            var parameters = await GetRedirectUrlParametersAsync(request, user, identity, userAgent, ipAddress, ct);
            var redirectUri = request.RedirectUri.AddParameters(parameters);

            return new PostAuthorizeResponse(redirectUri);
        }

        public async Task<TokenResponse> GetTokenAsync(
            TokenRequest request,
            ClaimsPrincipal claimsPrincipal,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            switch (request.GrantType)
            {
                case AuthorizeGrandType.AuthorizationCode:
                    var identityId = _hotStorage.GetValue<Guid>(request.Code);
                    var identityByCode = await _identitiesService.GetAsync(identityId, ct);
                    if (identityByCode == null)
                    {
                        return new TokenResponse("Invalid code");
                    }

                    var userByCode = await _usersService.GetAsync(identityByCode.UserId, ct);
                    if (userByCode == null)
                    {
                        return new TokenResponse("Invalid code");
                    }

                    var accessTokenByCode =
                        await CreateAccessTokenAsync(request.RedirectUri, userByCode, identityByCode, userAgent,
                            ipAddress, ct);
                    var refreshTokenByCode = await CreateRefreshTokenAsync(identityByCode, userAgent, ipAddress, ct);

                    return new TokenResponse(accessTokenByCode.Value, refreshTokenByCode.Value, "bearer",
                        TimeSpan.FromMinutes(30).Seconds);

                case AuthorizeGrandType.Password:
                    var identityTypes = IdentityTypeExtensions.TypesWithPassword;
                    var identityByPassword =
                        await _identitiesService.GetByKeyAndTypesAsync(request.Username, identityTypes, ct);
                    if (identityByPassword == null)
                    {
                        return new TokenResponse("Invalid credentials");
                    }

                    var userByPassword = await _usersService.GetAsync(identityByPassword.UserId, ct);
                    if (userByPassword == null)
                    {
                        return new TokenResponse("Invalid credentials");
                    }

                    var isPasswordCorrect = _identitiesService.IsPasswordCorrect(identityByPassword, request.Password);
                    if (!isPasswordCorrect)
                    {
                        return new TokenResponse("Invalid credentials");
                    }

                    var accessTokenByPassword =
                        await CreateAccessTokenAsync(request.RedirectUri, userByPassword, identityByPassword, userAgent,
                            ipAddress, ct);
                    var refreshTokenByPassword =
                        await CreateRefreshTokenAsync(identityByPassword, userAgent, ipAddress, ct);

                    return new TokenResponse(accessTokenByPassword.Value, refreshTokenByPassword.Value, "bearer",
                        TimeSpan.FromMinutes(30).Seconds);

                case AuthorizeGrandType.RefreshToken:

                    var oldRefreshToken = await _identityTokensService.GetByValueAsync(IdentityTokenType.RefreshToken,
                        request.RefreshToken, ct);

                    if (oldRefreshToken.ExpirationDateTime > DateTime.UtcNow)
                    {
                        return new TokenResponse("Refresh token is expired");
                    }

                    var identityByRefresh = await _identitiesService.GetAsync(oldRefreshToken.IdentityId, ct);
                    var userByRefresh = await _usersService.GetAsync(identityByRefresh.UserId, ct);

                    var accessTokenByRefresh =
                        await CreateAccessTokenAsync(request.RedirectUri, userByRefresh, identityByRefresh, userAgent,
                            ipAddress, ct);
                    var refreshTokenByRefresh =
                        await CreateRefreshTokenAsync(identityByRefresh, userAgent, ipAddress, ct);

                    return new TokenResponse(accessTokenByRefresh.Value, refreshTokenByRefresh.Value, "bearer",
                        TimeSpan.FromMinutes(30).Seconds);

                default:
                    return new TokenResponse("Invalid grand type");
            }
        }

        private async Task<(string key, object value)[]> GetRedirectUrlParametersAsync(
            PostAuthorizeRequest request,
            User user,
            Identities.Models.Identity identity,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            switch (request.ResponseType)
            {
                case AuthorizeResponseType.Code:
                    return GetCodeUriParameters(identity);
                case AuthorizeResponseType.Token:
                    return await GetTokensUriParametersAsync(request, user, identity, userAgent, ipAddress, ct);
                default:
                    throw new ArgumentOutOfRangeException(request.ResponseType);
            }
        }

        private (string key, object value)[] GetCodeUriParameters(Identities.Models.Identity identity)
        {
            var code = Generator.GenerateAlphaNumericString(8);

            _hotStorage.SetValue(code, identity.Id, TimeSpan.FromMinutes(10));

            return new (string key, object value)[]
            {
                ("code", code)
            };
        }

        private async Task<(string key, object value)[]> GetTokensUriParametersAsync(
            PostAuthorizeRequest request,
            User user,
            Identities.Models.Identity identity,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            var accessToken =
                await CreateAccessTokenAsync(request.RedirectUri, user, identity, userAgent, ipAddress, ct);
            var refreshToken = await CreateRefreshTokenAsync(identity, userAgent, ipAddress, ct);

            return new (string key, object value)[]
            {
                ("token_type", "bearer"),
                ("access_token", accessToken.Value),
                ("refresh_token", refreshToken.Value),
                ("expires_in", TimeSpan.FromDays(1).TotalSeconds)
            };
        }

        private async Task<IdentityToken> CreateAccessTokenAsync(
            string redirectUri,
            User user,
            Identities.Models.Identity identity,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var audience = new Uri(redirectUri).Host;
            var credentials = new SigningCredentials(AuthOptions.GetKey(), SecurityAlgorithms.HmacSha256);
            var claims = await GetClaimsAsync(user, ct);

            var jwt = new JwtSecurityToken("Identity", audience, notBefore: now, claims: claims,
                expires: now.AddMinutes(30), signingCredentials: credentials);

            var accessToken = new IdentityToken(identity.Id, IdentityTokenType.RefreshToken,
                new JwtSecurityTokenHandler().WriteToken(jwt), DateTime.UtcNow.AddMinutes(30), userAgent, ipAddress);

            await _identityTokensService.CreateAsync(accessToken, ct);

            return accessToken;
        }

        private async Task<IdentityToken> CreateRefreshTokenAsync(
            Identities.Models.Identity identity,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            var refreshToken = new IdentityToken(identity.Id, IdentityTokenType.RefreshToken,
                Generator.GenerateAlphaNumericString(32), DateTime.UtcNow.AddDays(60), userAgent, ipAddress);

            await _identityTokensService.CreateAsync(refreshToken, ct);

            return refreshToken;
        }

        private async Task<Claim[]> GetClaimsAsync(User user, CancellationToken ct)
        {
            var parameter = new IdentityGetPagedListParameter(user.Id, new[]
                {
                    IdentityType.EmailAndPassword,
                    IdentityType.PhoneAndPassword
                },
                true);
            var allIdentities = await _identitiesService.GetPagedListAsync(parameter, ct);
            var email = allIdentities.FirstOrDefault(x => x.Type == IdentityType.EmailAndPassword)?.Key;
            var phone = allIdentities.FirstOrDefault(x => x.Type == IdentityType.PhoneAndPassword)?.Key;

            return new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.DateOfBirth, user.BirthDate?.ToString("dd.MM.yyyy")),
                new Claim(ClaimTypes.Gender, user.Gender.ToString().ToLower()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.HomePhone, phone)
            };
        }
    }
}