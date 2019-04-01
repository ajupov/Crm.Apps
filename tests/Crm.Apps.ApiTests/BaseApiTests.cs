using System;
using Crm.Clients.Accounts;
using Crm.Infrastructure.Di;
using Microsoft.Extensions.Configuration;
using ConfigurationExtensions = Crm.Infrastructure.Configuration.ConfigurationExtensions;

namespace Crm.Apps.ApiTests
{
    public class BaseApiTests
    {
        protected readonly IConfiguration Configuration;
        protected readonly IServiceProvider ServiceProvider;

        protected BaseApiTests()
        {
            Configuration = ConfigurationExtensions.GetConfiguration();

            ServiceProvider = DiExtensions.GetServiceCollection()
                .ConfigureAccountsClient(Configuration)
                .GetServiceProvider();
        }
    }
}