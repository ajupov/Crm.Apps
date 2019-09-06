using System.Threading.Tasks;
using Crm.Apps.Accounts.Services;
using Crm.Apps.Accounts.Storages;
using Crm.Apps.Activities.Services;
using Crm.Apps.Activities.Storages;
using Crm.Apps.Companies.Services;
using Crm.Apps.Companies.Storages;
using Crm.Apps.Contacts.Services;
using Crm.Apps.Contacts.Storages;
using Crm.Apps.Deals.Services;
using Crm.Apps.Deals.Storages;
using Crm.Apps.Leads.Services;
using Crm.Apps.Leads.Storages;
using Crm.Apps.Products.Services;
using Crm.Apps.Products.Storages;
using Crm.Apps.Users.Services;
using Crm.Apps.Users.Storages;
using Crm.Common.UserContext;
using Crm.Infrastructure.ApiDocumentation;
using Crm.Infrastructure.Configuration;
using Crm.Infrastructure.Hosting;
using Crm.Infrastructure.HotStorage;
using Crm.Infrastructure.Logging;
using Crm.Infrastructure.Migrations;
using Crm.Infrastructure.Mvc;
using Crm.Infrastructure.OAuthClients;
using Crm.Infrastructure.Orm;
using Crm.Infrastructure.UserContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Apps
{
    public static class Program
    {
        public static Task Main()
        {
            return
                ConfigurationExtensions.GetConfiguration()
                    .ConfigureHost()
                    .ConfigureLogging()
                    .ConfigureServices((builder, services) => services
                        .ConfigureMvc()
//                        .ConfigureTracing()
                        .ConfigureApiDocumentation()
//                        .ConfigureMetrics(builder.Configuration)
                        .ConfigureMigrator(builder.Configuration)
                        .ConfigureOAuthClients(builder.Configuration)
                        .ConfigureOrm<AccountsStorage>(builder.Configuration)
                        .ConfigureOrm<UsersStorage>(builder.Configuration)
//                        .ConfigureOrm<IdentitiesStorage>(builder.Configuration)
                        .ConfigureOrm<ProductsStorage>(builder.Configuration)
                        .ConfigureOrm<LeadsStorage>(builder.Configuration)
                        .ConfigureOrm<CompaniesStorage>(builder.Configuration)
                        .ConfigureOrm<ContactsStorage>(builder.Configuration)
                        .ConfigureOrm<DealsStorage>(builder.Configuration)
                        .ConfigureOrm<ActivitiesStorage>(builder.Configuration)
                        .ConfigureHotStorage(builder.Configuration)
                        .ConfigureUserContext<IUserContext, UserContext>()
                        .AddTransient<IAccountsService, AccountsService>()
                        .AddTransient<IAccountChangesService, AccountChangesService>()
                        .AddTransient<IUsersService, UsersService>()
                        .AddTransient<IUserChangesService, UserChangesService>()
                        .AddTransient<IUserAttributesService, UserAttributesService>()
                        .AddTransient<IUserAttributeChangesService, UserAttributeChangesService>()
                        .AddTransient<IUserGroupsService, UserGroupsService>()
                        .AddTransient<IUserGroupChangesService, UserGroupChangesService>()
//                        .AddTransient<IIdentitiesService, IdentitiesService>()
//                        .AddTransient<IIdentityTokensService, IdentityTokensService>()
//                        .AddTransient<IIdentityChangesService, IdentityChangesService>()
                        .AddTransient<IProductsService, ProductsService>()
                        .AddTransient<IProductChangesService, ProductChangesService>()
                        .AddTransient<IProductCategoriesService, ProductCategoriesService>()
                        .AddTransient<IProductCategoryChangesService, ProductCategoryChangesService>()
                        .AddTransient<IProductStatusesService, ProductStatusesService>()
                        .AddTransient<IProductStatusChangesService, ProductStatusChangesService>()
                        .AddTransient<IProductAttributesService, ProductAttributesService>()
                        .AddTransient<IProductAttributeChangesService, ProductAttributeChangesService>()
                        .AddTransient<ILeadsService, LeadsService>()
                        .AddTransient<ILeadChangesService, LeadChangesService>()
                        .AddTransient<ILeadCommentsService, LeadCommentsService>()
                        .AddTransient<ILeadSourcesService, LeadSourcesService>()
                        .AddTransient<ILeadSourceChangesService, LeadSourceChangesService>()
                        .AddTransient<ILeadAttributesService, LeadAttributesService>()
                        .AddTransient<ILeadAttributeChangesService, LeadAttributeChangesService>()
                        .AddTransient<ICompaniesService, CompaniesService>()
                        .AddTransient<ICompanyChangesService, CompanyChangesService>()
                        .AddTransient<ICompanyAttributesService, CompanyAttributesService>()
                        .AddTransient<ICompanyAttributeChangesService, CompanyAttributeChangesService>()
                        .AddTransient<ICompanyCommentsService, CompanyCommentsService>()
                        .AddTransient<IContactsService, ContactsService>()
                        .AddTransient<IContactChangesService, ContactChangesService>()
                        .AddTransient<IContactAttributesService, ContactAttributesService>()
                        .AddTransient<IContactAttributeChangesService, ContactAttributeChangesService>()
                        .AddTransient<IContactCommentsService, ContactCommentsService>()
                        .AddTransient<IDealsService, DealsService>()
                        .AddTransient<IDealChangesService, DealChangesService>()
                        .AddTransient<IDealCommentsService, DealCommentsService>()
                        .AddTransient<IDealStatusesService, DealStatusesService>()
                        .AddTransient<IDealStatusChangesService, DealStatusChangesService>()
                        .AddTransient<IDealTypesService, DealTypesService>()
                        .AddTransient<IDealTypeChangesService, DealTypeChangesService>()
                        .AddTransient<IDealAttributesService, DealAttributesService>()
                        .AddTransient<IDealAttributeChangesService, DealAttributeChangesService>()
                        .AddTransient<IActivitiesService, ActivitiesService>()
                        .AddTransient<IActivityChangesService, ActivityChangesService>()
                        .AddTransient<IActivityCommentsService, ActivityCommentsService>()
                        .AddTransient<IActivityStatusesService, ActivityStatusesService>()
                        .AddTransient<IActivityStatusChangesService, ActivityStatusChangesService>()
                        .AddTransient<IActivityTypesService, ActivityTypesService>()
                        .AddTransient<IActivityTypeChangesService, ActivityTypeChangesService>()
                        .AddTransient<IActivityAttributesService, ActivityAttributesService>()
                        .AddTransient<IActivityAttributeChangesService, ActivityAttributeChangesService>())
                    .Configure(builder => builder
//                            .UseApiDocumentationsMiddleware()
                            .UseMigrationsMiddleware()
//                        .UseMetricsMiddleware()
                            .UseMvcMiddleware()
//                        .UseOAuthClients()
                    )
                    .Build()
                    .RunAsync();
        }
    }
}