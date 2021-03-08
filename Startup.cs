using Ajupov.Infrastructure.All.ApiDocumentation;
using Ajupov.Infrastructure.All.Cookies;
using Ajupov.Infrastructure.All.Cors;
using Ajupov.Infrastructure.All.HotStorage;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Metrics;
using Ajupov.Infrastructure.All.Migrations;
using Ajupov.Infrastructure.All.Mvc;
using Ajupov.Infrastructure.All.Mvc.Filters;
using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Tracing;
using Ajupov.Infrastructure.All.UserContext;
using Crm.Apps.Activities.Services;
using Crm.Apps.Activities.Storages;
using Crm.Apps.Auth.Settings;
using Crm.Apps.Companies.Services;
using Crm.Apps.Companies.Storages;
using Crm.Apps.Contacts.Services;
using Crm.Apps.Contacts.Storages;
using Crm.Apps.Deals.Services;
using Crm.Apps.Deals.Storages;
using Crm.Apps.Flags.Services;
using Crm.Apps.Flags.Storages;
using Crm.Apps.Leads.Services;
using Crm.Apps.Leads.Storages;
using Crm.Apps.Products.Services;
using Crm.Apps.Products.Storages;
using Crm.Apps.Settings.Services;
using Crm.Apps.Settings.Storages;
using Crm.Common.All.UserContext;
using LiteCrm.OAuth.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthorization()
                .AddJwtAuthentication()
                .AddJwtValidator(Configuration)
                .AddLiteCrmOAuth(Configuration)
                .AddCookieDefaults();

            services
                .AddCookiePolicy()
                .AddSingleOriginCorsPolicy(Configuration)
                .AddMvc(typeof(ValidationFilter))
                .AddTracing(Configuration)
                .AddApiDocumentation()
                .AddMetrics(Configuration)
                .AddMigrator(Configuration)
                .AddOrm<ProductsStorage>(Configuration)
                .AddOrm<LeadsStorage>(Configuration)
                .AddOrm<CompaniesStorage>(Configuration)
                .AddOrm<ContactsStorage>(Configuration)
                .AddOrm<DealsStorage>(Configuration)
                .AddOrm<ActivitiesStorage>(Configuration)
                .AddOrm<FlagsStorage>(Configuration)
                .AddOrm<SettingsStorage>(Configuration)
                .AddUserContext<IUserContext, UserContext>()
                .AddHotStorage(Configuration);

            services
                .Configure<AuthSettings>(Configuration.GetSection(nameof(AuthSettings)));

            services
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
                .AddTransient<IActivityAttributeChangesService, ActivityAttributeChangesService>();

            services
                .AddTransient<IAccountFlagsService, AccountFlagsService>()
                .AddTransient<IUserFlagsService, UserFlagsService>()
                .AddTransient<IAccountSettingsService, AccountSettingsService>()
                .AddTransient<IAccountSettingChangesService, AccountSettingChangesService>()
                .AddTransient<IUserSettingsService, UserSettingsService>()
                .AddTransient<IUserSettingChangesService, UserSettingChangesService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage()
                    .UseForwardedHeaders()
                    .UseHttpsRedirection()
                    .UseHsts();
            }

            app.UseApiDocumentationsMiddleware()
                .UseMigrationsMiddleware()
                .UseMetricsMiddleware()
                .UseCookiePolicy()
                .UseSingleOriginCors()
                .UseAuthentication()
                .UseAuthorization()
                .UseMvcMiddleware();
        }
    }
}
