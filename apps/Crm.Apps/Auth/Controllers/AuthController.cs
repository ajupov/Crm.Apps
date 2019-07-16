using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Crm.Apps.Identities.Models;
using Crm.Apps.Identities.Storages;
using Crm.Apps.Users.Models;
using Crm.Apps.Users.Storages;
using Crm.Infrastructure.HotStorage.HotStorage;
using Crm.Infrastructure.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Auth.Controllers
{
    [ApiController]
    [Route("Api/Auth")]
    public class AuthController : DefaultController
    {
        private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;
        private readonly IHotStorage _hotStorage;
        private readonly IRegistrationService _registrationService;
        private readonly UsersStorage _usersStorage;
        private readonly IdentitiesStorage _identitiesStorage;
        private readonly IAuthenticationService _authenticationService;

        public AuthController(
            IAuthenticationSchemeProvider authenticationSchemeProvider,
            IRegistrationService registrationService,
            IHotStorage hotStorage,
            UsersStorage usersStorage,
            IdentitiesStorage identitiesStorage,
            Businness.Authentication.IAuthenticationService authenticationService)
        {
            _authenticationSchemeProvider = authenticationSchemeProvider;
            _registrationService = registrationService;
            _hotStorage = hotStorage;
            _usersStorage = usersStorage;
            _identitiesStorage = identitiesStorage;
            _authenticationService = authenticationService;
        }

        [HttpGet("GetProviders")]
        public async Task<IEnumerable<object>> GetProviders()
        {
            var schemes = await _authenticationSchemeProvider.GetAllSchemesAsync().ConfigureAwait(false);

            return schemes.Where(x => !x.DisplayName.IsNullOrWhiteSpace()).Select(x => new
            {
                x.Name,
                x.DisplayName
            });
        }

        [HttpPost("GetProviders")]
        [HttpPost(Route.DefaultAction), ValidateModel]
        public async Task<SignInResponseModel> SignIn([FromBody] SignInRequestModel model)
        {
            var error = new SignInResponseModel("Не верный логин или пароль");

            var identity = await GetIdentityAsync(model).ConfigureAwait(false);
            if (identity == null)
            {
                return error;
            }

            var user = await GetUserAsync(identity).ConfigureAwait(false);
            if (user == null)
            {
                return error;
            }

            if (identity.PasswordHash.IsNullOrWhiteSpace())
            {
                return error;
            }

            if (!model.Password.IsVerifiedPassword(identity.PasswordHash))
            {
                return error;
            }

            await _authenticationService.SignInAsync(HttpContext, user, model.IsRemember).ConfigureAwait(false);

            return new SignInResponseModel();
        }

        [HttpPost(Route.DefaultAction), ValidateModel]
        public async Task<ChallengeResult> SignInExternal([FromForm] SignInExternalRequestModel model)
        {
            var state = Generator.GenerateAlphaNumbericString(8);
            var nonce = Generator.GenerateAlphaNumbericString(20);
            var storageValue = GetStorageValue(state, nonce);

            var saved = await _hotStorage.SaveAsync(storageValue, storageValue).ConfigureAwait(false);
            if (!saved)
            {
                throw new Exception($"Cannot save {nameof(state)} and {nameof(nonce)} in {nameof(_hotStorage)}.");
            }

            var properties = new AuthenticationProperties
            {
                RedirectUri = GenerateUrl("Authentication", nameof(SignInExternalCalback),
                    new SignInExternalCalbackRequestModel
                    {
                        RedirectUrl = model.RedirectUrl,
                        State = state,
                        Nonce = nonce
                    }),
                IsPersistent = true
            };

            return Challenge(properties, model.Provider);
        }

        [HttpGet(Route.DefaultAction), ValidateModel]
        public async Task<IActionResult> SignInExternalCalback([FromQuery] SignInExternalCalbackRequestModel model)
        {
            var storageValue = GetStorageValue(model.State, model.Nonce);

            var isExist = await _hotStorage.IsExistAsync(storageValue).ConfigureAwait(false);
            if (!isExist)
            {
                throw new Exception(
                    $"Cannot get {nameof(model.State)} and {nameof(model.Nonce)} from {nameof(_hotStorage)}.");
            }

            var user = await GetOrRegisterUserAsync().ConfigureAwait(false);
            if (user == null)
            {
                throw new Exception($"Cannot get or register {nameof(user)}.");
            }

            await _authenticationService.SignInAsync(HttpContext, user, false).ConfigureAwait(false);

            return RedirectToLocal(model.RedirectUrl);
        }

        [HttpPost(Route.DefaultAction)]
        public void SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        #region NonActions

        [NonAction]
        private async Task<User> GetOrRegisterUserAsync()
        {
            var identityType = Request.HttpContext.User.Identity.AuthenticationType.ToIdentityType();
            var externaId = Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var name = Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
            var surname = Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
            var genderFromClaims = Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Gender)?.Value;
            var gender = genderFromClaims == "male" ? UserGender.Male : genderFromClaims == "female" ? UserGender.Female : UserGender.None;
            var birthDate = Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.DateOfBirth)?.Value?.ToDate();

            User user;

            var identity = _identitiesStorage.Identities.FirstOrDefault(x => x.Type == identityType && x.Key == externaId);
            if (identity != null)
            {
                user = await _usersStorage.Users.FindAsync(identity.UserId).ConfigureAwait(false);
            }
            else
            {
                identity = _identitiesStorage.Identities.FirstOrDefault(x => x.Type == IdentityType.EmailAndPassword && x.Key == email);
                if (identity != null)
                {
                    user = await _usersStorage.Users.FindAsync(identity.UserId).ConfigureAwait(false);
                }
                else
                {
                    var account = await _registrationService.CreateAccountAsync().ConfigureAwait(false);
                    user = await _registrationService.CreateUserAsync(account, surname, name, birthDate, gender).ConfigureAwait(false);
                    await _registrationService.CreateEmailIdentityAsync(user, email, null).ConfigureAwait(false);
                }

                await _registrationService.CreateExternalIdentityAsync(user, identityType, externaId).ConfigureAwait(false);
            }

            return user;
        }

        [NonAction]
        private Task<Identity> GetIdentityAsync(SignInRequestModel model)
        {
            return _identitiesStorage.Identities.FirstOrDefaultAsync(x =>
                x.Type.IsWithPassword() && x.Key == model.Key);
        }

        [NonAction]
        private Task<User> GetUserAsync(Identity identity)
        {
            return _usersStorage.Users.FindAsync(identity.UserId);
        }

        [NonAction]
        private static string GetStorageValue(string state, string nonce)
        {
            return state + "_" + nonce;
        }

        #endregion
    }
}