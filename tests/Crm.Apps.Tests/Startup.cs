using Crm.Apps.Tests.Dsl.Builders.Account;
using Crm.Apps.Tests.Dsl.Builders.Identity;
using Crm.Apps.Tests.Dsl.Builders.IdentityToken;
using Crm.Apps.Tests.Dsl.Builders.User;
using Crm.Apps.Tests.Dsl.Builders.UserAttribute;
using Crm.Apps.Tests.Dsl.Builders.UserGroup;
using Crm.Apps.Tests.Dsl.Creator;
using Crm.Clients.Accounts;
using Crm.Clients.Identities;
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
                .AddTransient<ICreate, Create>()
                .AddTransient<IAccountBuilder, AccountBuilder>()
                .AddTransient<IUserBuilder, UserBuilder>()
                .AddTransient<IUserAttributeBuilder, UserAttributeBuilder>()
                .AddTransient<IUserGroupBuilder, UserGroupBuilder>()
                .AddTransient<IIdentityBuilder, IdentityBuilder>()
                .AddTransient<IIdentityTokenBuilder, IdentityTokenBuilder>();
        }
    }
}