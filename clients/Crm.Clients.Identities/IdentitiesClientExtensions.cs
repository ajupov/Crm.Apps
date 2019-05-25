using Crm.Clients.Identities.Clients.Identities;
using Crm.Clients.Identities.Clients.IdentityTokens;
using Crm.Clients.Identities.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Clients.Identities
{
    public static class IdentitiesClientExtensions
    {
        public static IServiceCollection ConfigureAccountsClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddHttpClient()
                .Configure<IdentitiesClientSettings>(configuration.GetSection("IdentitiesClientSettings"))
                .AddSingleton<IIdentitiesClient, IdentitiesClient>()
                .AddSingleton<IIdentityTokensClient, IdentityTokensClient>();
        }
    }
}