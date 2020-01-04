//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Security.Principal;
//using System.Threading;
//using System.Threading.Tasks;
//using Crm.Apps.Accounts.Models;
//using Crm.Clients.Identities.Clients;
//using Crm.Clients.Identities.Models;
//using Crm.Clients.Users.Clients;
//using Crm.Clients.Users.Models;
//using Crm.Infrastructure.HotStorage.HotStorage;
//using Crm.Utils.DateTime;
//using Identity.OAuth.Models;
//using Microsoft.AspNetCore.Authentication;
//
//namespace Crm.Apps.Auth.Services
//{
//    public class AuthService : IAuthService
//    {
//        private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;
////        private readonly IAccountsClient _accountsClient;
//        private readonly IUsersClient _usersClient;
//        private readonly IdentitiesClient _identitiesClient;
//        private readonly IHotStorage _hotStorage;
//
//        public AuthService(
//            IAuthenticationSchemeProvider authenticationSchemeProvider,
////            IAccountsClient accountsClient,
//            IUsersClient usersClient,
//            IdentitiesClient identitiesClient,
//            IHotStorage hotStorage)
//        {
//            _authenticationSchemeProvider = authenticationSchemeProvider;
////            _accountsClient = accountsClient;
//            _usersClient = usersClient;
//            _identitiesClient = identitiesClient;
//            _hotStorage = hotStorage;
//        }
//
//        public async Task<Dictionary<string, string>> GetProvidersAsync()
//        {
//            var schemes = await _authenticationSchemeProvider.GetAllSchemesAsync();
//
//            return schemes.ToDictionary(k => k.DisplayName, v => v.Name);
//        }
//
//        public AuthenticationProperties GetExternalProperties(string ipAddress, string state, string callbackUri)
//        {
//            _hotStorage.SetValue(ipAddress, state, TimeSpan.FromMinutes(10));
//
//            return new AuthenticationProperties
//            {
//                IsPersistent = true,
//                RedirectUri = callbackUri
//            };
//        }
//
//        public async Task<CallbackResponse> CallbackAsync(
//            CallbackRequest request,
//            ClaimsPrincipal claimsPrincipal,
//            string ipAddress,
//            CancellationToken ct)
//        {
//            var identityType = await GetIdentityTypeAsync(claimsPrincipal, ct);
//            var user = await GetOrRegisterUserAsync(claimsPrincipal, identityType, ct);
//
//            return new CallbackResponse("" /*,user.Name, clientIpAddress, request.RedirectUrl*/);
//        }
//
//        public void SignOut()
//        {
//        }
//
//        private async Task<IdentityType> GetIdentityTypeAsync(IPrincipal claimsPrincipal, CancellationToken ct)
//        {
//            var types = await _identitiesClient.GetTypesAsync(ct);
//
//            return types[claimsPrincipal.Identity.AuthenticationType];
//        }
//
//        private async Task<User> GetOrRegisterUserAsync(
//            ClaimsPrincipal claimsPrincipal,
//            IdentityType identityType,
//            CancellationToken ct)
//        {
//            var externalId = GetClaimValue(claimsPrincipal, ClaimTypes.NameIdentifier);
//            var email = GetClaimValue(claimsPrincipal, ClaimTypes.Email);
//            var name = GetClaimValue(claimsPrincipal, ClaimTypes.GivenName);
//            var surname = GetClaimValue(claimsPrincipal, ClaimTypes.Surname);
//            var birthDate = GetClaimValue(claimsPrincipal, ClaimTypes.DateOfBirth).ToDate();
//            var gender = GetGender(GetClaimValue(claimsPrincipal, ClaimTypes.Gender));
//
//            User user;
//
//            var externalIdIdentity =
//                await _identitiesClient.GetByKeyAndTypesAsync(externalId, new[] {identityType}, ct);
//            var emailIdentity =
//                await _identitiesClient.GetByKeyAndTypesAsync(email, new[] {IdentityType.EmailAndPassword}, ct);
//
//            if (externalIdIdentity != null)
//            {
//                user = await _usersClient.GetAsync(externalIdIdentity.UserId, ct);
//
//                if (emailIdentity == null)
//                {
////                    emailIdentity = new Identity(user.Id, IdentityType.EmailAndPassword, null, false, false);
//                    await _identitiesClient.CreateAsync(emailIdentity, ct);
//                }
//            }
//            else if (emailIdentity != null)
//            {
//                user = await _usersClient.GetAsync(emailIdentity.UserId, ct);
//            }
//            else
//            {
//                var account = new Account(AccountType.MlmSystem);
////                var accountId = await _accountsClient.CreateAsync(account, ct);
//
////                var newUser = new User(accountId, surname, name, null, birthDate, gender);
////                var userId = await _usersClient.CreateAsync(newUser, ct);
////                user = await _usersClient.GetAsync(userId, ct);
//
////                externalIdIdentity = new Identity(user.Id, identityType, externalId, false, true);
//                await _identitiesClient.CreateAsync(externalIdIdentity, ct);
//
////                emailIdentity = new Identity(user.Id, IdentityType.EmailAndPassword, email, false, true);
//                await _identitiesClient.CreateAsync(emailIdentity, ct);
//            }
//
////            return user;
//        }
//
//        private static string GetClaimValue(
//            ClaimsPrincipal claimsPrincipal,
//            string type)
//        {
//            return claimsPrincipal.Claims.FirstOrDefault(x => x.Type == type)?.Value;
//        }
//
//        private static UserGender GetGender(string gender)
//        {
//            switch (gender?.ToLower())
//            {
//                case "male":
//                    return UserGender.Male;
//                case "female":
//                    return UserGender.Female;
//                default:
//                    throw new ArgumentOutOfRangeException(nameof(gender));
//            }
//        }
//    }
//}