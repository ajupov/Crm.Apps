using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Crm.Tests
{
    public class AccountsTest
    {
        [Fact]
        public async Task Get()
        {
            var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

            using (var client = httpClientFactory.CreateClient())
            {
                var result = await client.GetAsync("localhost:9000/Api/Accounts");

                Assert.True(result.IsSuccessStatusCode);
            }
        }
    }
}