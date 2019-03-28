using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Crm.Common.OAuth.Odnoklassniki
{
    public class OdnoklassnikiAuthenticationHandler : OAuthHandler<OdnoklassnikiAuthenticationOptions>
    {
        public OdnoklassnikiAuthenticationHandler(IOptionsMonitor<OdnoklassnikiAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity,
            AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var md5Inner = GetMd5(tokens.AccessToken + Options.ClientSecret);
            var parametersForSig = PrepareParametersForSig(md5Inner);
            var sig = GetMd5(parametersForSig);
            var address = PrepareUserInfoAddress(tokens.AccessToken, sig);

            var response = await Backchannel.GetAsync(address, Context.RequestAborted).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                await LogErrorFromResponseAsync(response).ConfigureAwait(false);

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            var principal = new ClaimsPrincipal(identity);

            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel,
                tokens, payload);

            context.RunClaimActions(payload);

            await Options.Events.CreatingTicket(context).ConfigureAwait(false);

            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        private static string GetMd5(string value)
        {
            var md5 = MD5.Create();

            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            var stringBuilder = new StringBuilder();

            foreach (var b in bytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        private string PrepareParametersForSig(string md5Inner)
        {
            return $"application_key={Options.ApplicationKey}" +
                   $"format={Options.Format}" +
                   $"method={Options.UserInfoMethod}" +
                   $"{md5Inner}";
        }

        private string PrepareUserInfoAddress(string accessToken, string sig)
        {
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "application_key",
                Options.ApplicationKey);

            address = QueryHelpers.AddQueryString(address, "format", "json");
            address = QueryHelpers.AddQueryString(address, "method", Options.UserInfoMethod);
            address = QueryHelpers.AddQueryString(address, "sig", sig);
            address = QueryHelpers.AddQueryString(address, "access_token", accessToken);

            return address;
        }

        private async Task LogErrorFromResponseAsync(HttpResponseMessage response)
        {
            const string error = "An error occurred while retrieving the user profile: the remote server " +
                                 "returned a {Status} response with the following payload: {Headers} {Body}.";

            var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            Logger.LogError(error, response.StatusCode, response.Headers.ToString(), body);
        }
    }
}