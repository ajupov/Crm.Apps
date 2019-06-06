using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Crm.Infrastructure.Hosting
{
    public static class HostExtensions
    {
        public static IWebHostBuilder ConfigureHost(this IConfiguration configuration)
        {
            return new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseUrls(configuration.GetValue<string>("ApplicationHost"))
                .UseKestrel();
        }
    }
}