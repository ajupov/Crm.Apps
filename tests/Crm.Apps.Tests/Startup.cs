using Crm.Clients.Accounts;
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
                .ConfigureAccountsClient(configuration);
            services
                .ConfigureUsersClient(configuration);
        }
    }
}