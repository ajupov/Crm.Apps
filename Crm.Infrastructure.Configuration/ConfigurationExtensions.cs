using System;
using Microsoft.Extensions.Configuration;

namespace Crm.Infrastructure.Configuration
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureAppConfiguration(this IConfigurationBuilder builder)
        {
            builder.SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", false)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
