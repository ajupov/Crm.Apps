using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Crm.Infrastructure.Host
{
    public static class HostExtensions
    {
        public static IWebHostBuilder ConfigureHost(this IWebHostBuilder webHostBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            return webHostBuilder
                .UseConfiguration(configuration)
                .UseKestrel(options => options.ListenLocalhost(5000));
        }
    }
}