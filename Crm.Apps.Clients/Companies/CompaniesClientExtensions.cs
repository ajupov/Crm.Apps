using Crm.Apps.Clients.Companies.Clients;
using Crm.Apps.Clients.Companies.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Apps.Clients.Companies
{
    public static class CompaniesClientExtensions
    {
        public static IServiceCollection ConfigureCompaniesClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddHttpClient()
                .Configure<CompaniesClientSettings>(configuration.GetSection("CompaniesClientSettings"))
                .AddSingleton<ICompaniesClient, CompaniesClient>()
                .AddSingleton<ICompanyChangesClient, CompanyChangesClient>()
                .AddSingleton<ICompanyAttributesClient, CompanyAttributesClient>()
                .AddSingleton<ICompanyAttributeChangesClient, CompanyAttributeChangesClient>()
                .AddSingleton<ICompanyCommentsClient, CompanyCommentsClient>();
        }
    }
}