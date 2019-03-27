using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Crm.Infrastructure.Host
{
    public static class HostExtensions
    {
        public static IWebHostBuilder ConfigureHost(this IWebHostBuilder webHostBuilder)
        {
            return webHostBuilder.ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.SetBasePath(Environment.CurrentDirectory)
                        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
                        .AddEnvironmentVariables()
                        .Build();
                })
                .UseKestrel();
        }
    }
}