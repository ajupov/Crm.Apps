using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Infrastructure.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection ConfigureConfiguration(this IServiceCollection services,
            WebHostBuilderContext webHostBuilder)
        {
            return services
                .AddSingleton(webHostBuilder.Configuration)
                .AddOptions();
        }
    }
}