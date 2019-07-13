using Crm.Apps.Tests.Builders.Accounts;
using Crm.Apps.Tests.Builders.Activities;
using Crm.Apps.Tests.Builders.Companies;
using Crm.Apps.Tests.Builders.Contacts;
using Crm.Apps.Tests.Builders.Deals;
using Crm.Apps.Tests.Builders.Identities;
using Crm.Apps.Tests.Builders.Leads;
using Crm.Apps.Tests.Builders.Products;
using Crm.Apps.Tests.Builders.Users;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Accounts;
using Crm.Clients.Activities;
using Crm.Clients.Companies;
using Crm.Clients.Contacts;
using Crm.Clients.Deals;
using Crm.Clients.Identities;
using Crm.Clients.Leads;
using Crm.Clients.Products;
using Crm.Clients.Users;
using Crm.Infrastructure.Configuration;
using Crm.Infrastructure.DependencyInjection.Tests;
using Crm.Infrastructure.DependencyInjection.Tests.Attributes;
using Microsoft.Extensions.DependencyInjection;

[assembly: DependencyInject("Crm.Apps.Tests.Startup", "Crm.Apps.Tests")]

namespace Crm.Apps.Tests
{
    public class Startup : BaseStartup
    {
        protected override void Configure(IServiceCollection services)
        {
            var configuration = ConfigurationExtensions.GetConfiguration();

            services
                .ConfigureAccountsClient(configuration)
                .ConfigureUsersClient(configuration)
                .ConfigureIdentitiesClient(configuration)
                .ConfigureProductsClient(configuration)
                .ConfigureLeadsClient(configuration)
                .ConfigureCompaniesClient(configuration)
                .ConfigureContactsClient(configuration)
                .ConfigureDealsClient(configuration)
                .ConfigureActivitiesClient(configuration)
                .AddTransient<ICreate, Create>()
                .AddTransient<IAccountBuilder, AccountBuilder>()
                .AddTransient<IUserBuilder, UserBuilder>()
                .AddTransient<IUserAttributeBuilder, UserAttributeBuilder>()
                .AddTransient<IUserGroupBuilder, UserGroupBuilder>()
                .AddTransient<IIdentityBuilder, IdentityBuilder>()
                .AddTransient<IIdentityTokenBuilder, IdentityTokenBuilder>()
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