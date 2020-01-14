using System.Threading.Tasks;
using Ajupov.Infrastructure.All.ApiDocumentation;
using Ajupov.Infrastructure.All.Hosting;
using Ajupov.Infrastructure.All.HotStorage;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Logging;
using Ajupov.Infrastructure.All.Metrics;
using Ajupov.Infrastructure.All.Migrations;
using Ajupov.Infrastructure.All.Mvc;
using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Tracing;
using Ajupov.Infrastructure.All.UserContext;
using Crm.Apps.Accounts.Services;
using Crm.Apps.Accounts.Storages;
using Crm.Apps.Activities.Services;
using Crm.Apps.Activities.Storages;
using Crm.Apps.Auth.ExtractAccessToken;
using Crm.Apps.Auth.Services;
using Crm.Apps.Companies.Services;
using Crm.Apps.Companies.Storages;
using Crm.Apps.Contacts.Services;
using Crm.Apps.Contacts.Storages;
using Crm.Apps.Deals.Services;
using Crm.Apps.Deals.Storages;
using Crm.Apps.Extensions;
using Crm.Apps.Leads.Services;
using Crm.Apps.Leads.Storages;
using Crm.Apps.Products.Services;
using Crm.Apps.Products.Storages;
using Crm.Apps.RefreshTokens.Services;
using Crm.Apps.RefreshTokens.Storages;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ConfigurationExtensions = Ajupov.Infrastructure.All.Configuration.ConfigurationExtensions;

namespace Crm.Apps
{
    public static class Program
    {
        public static Task Main()
        {
            var configuration = ConfigurationExtensions.GetConfiguration();

            return configuration
                .ConfigureHost()
                .ConfigureLogging(configuration)
                .ConfigureServices((builder, services) =>
                {
                    services
                        .AddAuthorization()
                        .AddJwtAuthentication()
                        .AddJwtValidator("7BA30F0F-44D9-4340-80F5-AC2717AFDD25", "http://localhost:9000")
                        .AddLiteCrmOAuth(configuration)
                        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

                    services
                        .ConfigureMvc()
                        .ConfigureTracing(configuration)
                        .ConfigureApiDocumentation()
                        .ConfigureMetrics(builder.Configuration)
                        .ConfigureMigrator(builder.Configuration)
                        .ConfigureOrm<AccountsStorage>(builder.Configuration)
                        .ConfigureOrm<ProductsStorage>(builder.Configuration)
                        .ConfigureOrm<LeadsStorage>(builder.Configuration)
                        .ConfigureOrm<CompaniesStorage>(builder.Configuration)
                        .ConfigureOrm<ContactsStorage>(builder.Configuration)
                        .ConfigureOrm<DealsStorage>(builder.Configuration)
                        .ConfigureOrm<ActivitiesStorage>(builder.Configuration)
                        .ConfigureOrm<RefreshTokensStorage>(builder.Configuration)
                        .ConfigureHotStorage(builder.Configuration)
                        .ConfigureUserContext<IUserContext, UserContext>();

                    services
                        .AddTransient<IAuthService, AuthService>()
                        .AddTransient<IAccountsService, AccountsService>()
                        .AddTransient<IAccountChangesService, AccountChangesService>()
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
                        .AddTransient<IActivityAttributeChangesService, ActivityAttributeChangesService>()
                        .AddTransient<IRefreshTokensService, RefreshTokensService>()
                        .AddTransient<ExtractAccessTokenMiddleware>();
                })
                .Configure((context, builder) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        builder.UseDeveloperExceptionPage();
                    }

                    builder
                        .UseApiDocumentationsMiddleware()
                        .UseMigrationsMiddleware()
                        .UseMetricsMiddleware()
                        .UseExtractAccessToken()
                        .UseAuthentication()
                        .UseAuthorization()
                        .UseMvcMiddleware();
                })
                .Build()
                .RunAsync();
        }
    }
}