using Ajupov.Infrastructure.All.TestsDependencyInjection;
using Ajupov.Infrastructure.All.TestsDependencyInjection.Attributes;
using Crm.Apps.Clients;
using Crm.Apps.Tests.Builders.Activities;
using Crm.Apps.Tests.Builders.Companies;
using Crm.Apps.Tests.Builders.Contacts;
using Crm.Apps.Tests.Builders.Deals;
using Crm.Apps.Tests.Builders.Leads;
using Crm.Apps.Tests.Builders.Products;
using Crm.Apps.Tests.Creator;
using Microsoft.Extensions.DependencyInjection;

[assembly: DependencyInject("Crm.Apps.Tests.Startup", "Crm.Apps.Tests")]

namespace Crm.Apps.Tests
{
    public class Startup : BaseStartup
    {
        protected override void Configure(IServiceCollection services)
        {
            services
                .ConfigureClients("http://localhost:9000", "http://localhost:3000")
                // .ConfigureClients("http://api.litecrm.org", "http://identity.litecrm.org")
                .AddTransient<ICreate, Create>()
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