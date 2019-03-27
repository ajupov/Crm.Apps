using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Infrastructure.Migrations
{
    public static class MigrationsExtensions
    {
        public static IServiceCollection ConfigureMigrator(this IServiceCollection services,
            IConfiguration configuration, Assembly callerAssembly)
        {
            services.AddFluentMigratorCore()
                .ConfigureRunner(builder =>
                    builder.AddPostgres()
                        .WithGlobalConnectionString(configuration.GetConnectionString("MigrationsConnectionString"))
                        .ScanIn(callerAssembly)
                        .For.Migrations())
                .AddLogging(builder => builder.AddFluentMigratorConsole());

            return services;
        }

        public static IApplicationBuilder UseMigrationsMiddleware(this IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                scope.ServiceProvider.GetService<IMigrationRunner>().MigrateUp();
            }

            return applicationBuilder;
        }
    }
}