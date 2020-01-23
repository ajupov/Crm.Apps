using Ajupov.Infrastructure.All.TestsDependencyInjection;
using Ajupov.Infrastructure.All.TestsDependencyInjection.Attributes;
using Crm.Apps.Tests.Builders.Activities;
using Crm.Apps.Tests.Builders.Companies;
using Crm.Apps.Tests.Builders.Contacts;
using Crm.Apps.Tests.Builders.Deals;
using Crm.Apps.Tests.Builders.Leads;
using Crm.Apps.Tests.Builders.Products;
using Crm.Apps.Tests.Services.AccessTokenGetter;
using Crm.Apps.Tests.Services.Creator;
using Crm.Apps.Tests.Settings;
using Crm.Apps.v1.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ConfigurationExtensions = Ajupov.Infrastructure.All.Configuration.ConfigurationExtensions;

[assembly: DependencyInject("Crm.Apps.Tests.Startup", "Crm.Apps.Tests")]

namespace Crm.Apps.Tests
{
    public class Startup : BaseStartup
    {
        protected override void Configure(IServiceCollection services)
        {
            var configuration = ConfigurationExtensions.GetConfiguration();

            var hostsSettings = configuration.GetSection(nameof(HostsSettings));
            var apiHost = hostsSettings.GetValue<string>(nameof(HostsSettings.ApiHost));
            var oauthHost = hostsSettings.GetValue<string>(nameof(HostsSettings.OAuthHost));

            services
                .ConfigureClients(apiHost, oauthHost)
                .Configure<OAuthSettings>(configuration.GetSection(nameof(OAuthSettings)));

            services.AddSingleton<IAccessTokenGetter, AccessTokenGetter>()
                .AddTransient<ICreate, Create>();

            services
                .AddTransient<IProductBuilder, ProductBuilder>()
                .AddTransient<IProductAttributeBuilder, ProductAttributeBuilder>()
                .AddTransient<IProductCategoryBuilder, ProductCategoryBuilder>()
                .AddTransient<IProductStatusBuilder, ProductStatusBuilder>()
                .AddTransient<ILeadBuilder, LeadBuilder>()
                .AddTransient<ILeadAttributeBuilder, LeadAttributeBuilder>()
                .AddTransient<ILeadCommentBuilder, LeadCommentBuilder>()
                .AddTransient<ILeadSourceBuilder, LeadSourceBuilder>()
                .AddTransient<ICompanyBuilder, CompanyBuilder>()
                .AddTransient<ICompanyAttributeBuilder, CompanyAttributeBuilder>()
                .AddTransient<ICompanyCommentBuilder, CompanyCommentBuilder>()
                .AddTransient<IContactBuilder, ContactBuilder>()
                .AddTransient<IContactAttributeBuilder, ContactAttributeBuilder>()
                .AddTransient<IContactCommentBuilder, ContactCommentBuilder>()
                .AddTransient<IDealBuilder, DealBuilder>()
                .AddTransient<IDealAttributeBuilder, DealAttributeBuilder>()
                .AddTransient<IDealCommentBuilder, DealCommentBuilder>()
                .AddTransient<IDealStatusBuilder, DealStatusBuilder>()
                .AddTransient<IDealTypeBuilder, DealTypeBuilder>()
                .AddTransient<IActivityBuilder, ActivityBuilder>()
                .AddTransient<IActivityAttributeBuilder, ActivityAttributeBuilder>()
                .AddTransient<IActivityCommentBuilder, ActivityCommentBuilder>()
                .AddTransient<IActivityStatusBuilder, ActivityStatusBuilder>()
                .AddTransient<IActivityTypeBuilder, ActivityTypeBuilder>();
        }
    }
}