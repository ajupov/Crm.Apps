using Crm.Apps.Clients.Activities.Clients;
using Crm.Apps.Clients.Companies.Clients;
using Crm.Apps.Clients.Contacts.Clients;
using Crm.Apps.Clients.Deals.Clients;
using Crm.Apps.Clients.Leads.Clients;
using Crm.Apps.Clients.Products.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Apps.Clients
{
    public static class ClientsExtensions
    {
        public static IServiceCollection ConfigureClients(this IServiceCollection services, string host)
        {
            return services
                // .AddHttpClient()
                .Configure<ClientsSettings>(x => { x.Host = host; })
                .AddSingleton<IActivitiesClient, ActivitiesClient>()
                .AddSingleton<IActivityChangesClient, ActivityChangesClient>()
                .AddSingleton<IActivityAttributesClient, ActivityAttributesClient>()
                .AddSingleton<IActivityAttributeChangesClient, ActivityAttributeChangesClient>()
                .AddSingleton<IActivityStatusesClient, ActivityStatusesClient>()
                .AddSingleton<IActivityStatusChangesClient, ActivityStatusChangesClient>()
                .AddSingleton<IActivityTypesClient, ActivityTypesClient>()
                .AddSingleton<IActivityTypeChangesClient, ActivityTypeChangesClient>()
                .AddSingleton<IActivityCommentsClient, ActivityCommentsClient>()
                .AddSingleton<ICompaniesClient, CompaniesClient>()
                .AddSingleton<ICompanyChangesClient, CompanyChangesClient>()
                .AddSingleton<ICompanyAttributesClient, CompanyAttributesClient>()
                .AddSingleton<ICompanyAttributeChangesClient, CompanyAttributeChangesClient>()
                .AddSingleton<ICompanyCommentsClient, CompanyCommentsClient>()
                .AddSingleton<IContactsClient, ContactsClient>()
                .AddSingleton<IContactChangesClient, ContactChangesClient>()
                .AddSingleton<IContactAttributesClient, ContactAttributesClient>()
                .AddSingleton<IContactAttributeChangesClient, ContactAttributeChangesClient>()
                .AddSingleton<IContactCommentsClient, ContactCommentsClient>()
                .AddSingleton<IDealsClient, DealsClient>()
                .AddSingleton<IDealChangesClient, DealChangesClient>()
                .AddSingleton<IDealAttributesClient, DealAttributesClient>()
                .AddSingleton<IDealAttributeChangesClient, DealAttributeChangesClient>()
                .AddSingleton<IDealStatusesClient, DealStatusesClient>()
                .AddSingleton<IDealStatusChangesClient, DealStatusChangesClient>()
                .AddSingleton<IDealTypesClient, DealTypesClient>()
                .AddSingleton<IDealTypeChangesClient, DealTypeChangesClient>()
                .AddSingleton<IDealCommentsClient, DealCommentsClient>()
                .AddSingleton<ILeadsClient, LeadsClient>()
                .AddSingleton<ILeadChangesClient, LeadChangesClient>()
                .AddSingleton<ILeadAttributesClient, LeadAttributesClient>()
                .AddSingleton<ILeadAttributeChangesClient, LeadAttributeChangesClient>()
                .AddSingleton<ILeadSourcesClient, LeadSourcesClient>()
                .AddSingleton<ILeadSourceChangesClient, LeadSourceChangesClient>()
                .AddSingleton<ILeadCommentsClient, LeadCommentsClient>()
                .AddSingleton<IProductsClient, ProductsClient>()
                .AddSingleton<IProductChangesClient, ProductChangesClient>()
                .AddSingleton<IProductAttributesClient, ProductAttributesClient>()
                .AddSingleton<IProductAttributeChangesClient, ProductAttributeChangesClient>()
                .AddSingleton<IProductCategoriesClient, ProductCategoriesClient>()
                .AddSingleton<IProductCategoryChangesClient, ProductCategoryChangesClient>()
                .AddSingleton<IProductStatusesClient, ProductStatusesClient>()
                .AddSingleton<IProductStatusChangesClient, ProductStatusChangesClient>();
        }
    }
}