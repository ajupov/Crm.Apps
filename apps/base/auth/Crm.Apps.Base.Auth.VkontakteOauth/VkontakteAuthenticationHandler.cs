using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Crm.Common.OAuth.Vkontakte
{
    public class VkontakteAuthenticationHandler : OAuthHandler<VkontakteAuthenticationOptions>
    {
        public VkontakteAuthenticationHandler(IOptionsMonitor<VkontakteAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity,
            AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var address =
                QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "access_token", tokens.AccessToken);

            address = QueryHelpers.AddQueryString(address, "v", Options.ApiVersion);

            if (Options.Fields.Count != 0)
            {
                address = QueryHelpers.AddQueryString(address, "fields", string.Join(",", Options.Fields));
            }

            var response = await Backchannel.GetAsync(address, Context.RequestAborted).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                await LogErrorFromResponseAsync(response).ConfigureAwait(false);

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            var container = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var payload = container["response"].First as JObject;

            if (tokens.Response["email"] != null)
            {
                payload.Add("email", tokens.Response["email"]);
            }

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel,
                tokens, payload);
            context.RunClaimActions(payload);

            await Options.Events.CreatingTicket(context).ConfigureAwait(false);

            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
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