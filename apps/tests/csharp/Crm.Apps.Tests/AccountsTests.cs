using System;
using System.Net.Http;
using System.Threading.Tasks;
using Crm.Infrastructure.Configuration;
using Crm.Infrastructure.Di;
using Crm.Utils.Http;
using Crm.Utils.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Crm.Apps.Tests
{
    [Collection("Accounts tests")]
    public class AccountsTests
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceProvider _services;
        private readonly string _host;

        public AccountsTests()
        {
            _configuration = Builder.GetConfiguration();
            _services = DiExtension.GetServiceCollection().AddHttpClient().GetServiceProvider();
            _host = _configuration["AccountsHost"];
        }

        [Fact(DisplayName = "Get")]
        public async Task Get()
        {
            using (var client = _services.GetService<IHttpClientFactory>().CreateClient())
            {
                var result = await client.GetAsync(_host);

                Assert.True(result.IsSuccessStatusCode);
            }
        }

        [Fact(DisplayName = "Create account")]
        public async Task Create()
        {
            using (var client = _services.GetService<IHttpClientFactory>().CreateClient())
            {
                var data = new { }.ToJsonStringContent();
                var result = await client.PostAsync($"{_host}/Api/Accounts/Create", data);

                Assert.True(result.IsSuccessStatusCode);

                var content = await result.Content.ReadAsStringAsync();
                var id = content.FromJsonString<Guid>();
                
                Assert.NotNull(id);
                Assert.NotEqual(id, Guid.Empty);
            }
        }
    }
}