using Crm.Clients.Deals.Clients;
using Crm.Clients.Deals.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Clients.Deals
{
    public static class DealsClientExtensions
    {
        public static IServiceCollection ConfigureDealsClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddHttpClient()
                .Configure<DealsClientSettings>(configuration.GetSection("DealsClientSettings"))
                .AddSingleton<IDealsClient, DealsClient>()
                .AddSingleton<IDealChangesClient, DealChangesClient>()
                .AddSingleton<IDealAttributesClient, DealAttributesClient>()
                .AddSingleton<IDealAttributeChangesClient, DealAttributeChangesClient>()
                .AddSingleton<IDealStatusesClient, DealStatusesClient>()
                .AddSingleton<IDealStatusChangesClient, DealStatusChangesClient>()
                .AddSingleton<IDealTypesClient, DealTypesClient>()
                .AddSingleton<IDealTypeChangesClient, DealTypeChangesClient>()
                .AddSingleton<IDealCommentsClient, DealCommentsClient>();
        }
    }
}