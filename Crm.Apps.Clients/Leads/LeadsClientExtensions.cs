using Crm.Clients.Leads.Clients;
using Crm.Clients.Leads.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Clients.Leads
{
    public static class LeadsClientExtensions
    {
        public static IServiceCollection ConfigureLeadsClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddHttpClient()
                .Configure<LeadsClientSettings>(configuration.GetSection("LeadsClientSettings"))
                .AddSingleton<ILeadsClient, LeadsClient>()
                .AddSingleton<ILeadChangesClient, LeadChangesClient>()
                .AddSingleton<ILeadAttributesClient, LeadAttributesClient>()
                .AddSingleton<ILeadAttributeChangesClient, LeadAttributeChangesClient>()
                .AddSingleton<ILeadSourcesClient, LeadSourcesClient>()
                .AddSingleton<ILeadSourceChangesClient, LeadSourceChangesClient>()
                .AddSingleton<ILeadCommentsClient, LeadCommentsClient>();
        }
    }
}