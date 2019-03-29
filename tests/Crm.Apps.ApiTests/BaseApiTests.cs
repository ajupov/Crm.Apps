using System.Net.Http;
using Crm.Infrastructure.Di;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ConfigurationExtensions = Crm.Infrastructure.Configuration.ConfigurationExtensions;

namespace Crm.Apps.ApiTests
{
    public class BaseApiTests
    {
        protected readonly IConfiguration Configuration;
        protected readonly IHttpClientFactory HttpClientFactory;

        protected BaseApiTests()
        {
            Configuration = ConfigurationExtensions.GetConfiguration();

            HttpClientFactory = DiExtensions
                .GetServiceCollection()
                .AddHttpClient()
                .GetServiceProvider()
                .GetService<IHttpClientFactory>();
        }
    }
}