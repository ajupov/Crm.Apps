using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Crm.Infrastructure.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IWebHostBuilder ConfigureHost(this IWebHostBuilder webHostBuilder)
        {
            return webHostBuilder.ConfigureAppConfiguration(builder =>
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                builder.SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile($"appsettings.{environment}.json")
                    .AddEnvironmentVariables()
                    .Build();
            })
            .UseKestrel();
        }
    }
}