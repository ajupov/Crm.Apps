using Crm.Clients.Accounts;
using Crm.Infrastructure.Configuration;
using Crm.Infrastructure.Di;
using Microsoft.Extensions.DependencyInjection;

[assembly: DiAttribute("Crm.Apps.Tests.Startup", "Crm.Apps.Tests")]

namespace Crm.Apps.Tests
{
    public class Startup : BaseStartup
    {
        protected override void Configure(IServiceCollection services)
        {
            services.ConfigureAccountsClient(ConfigurationExtensions.GetConfiguration());
        }
    }
}