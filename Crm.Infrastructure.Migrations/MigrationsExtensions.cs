﻿using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Infrastructure.Migrations
{
    public static class MigrationsExtensions
    {
        public static IServiceCollection ConfigureMigrator(this IServiceCollection services, string connectionString,
            Assembly callerAssembly)
        {
            services.AddFluentMigratorCore().ConfigureRunner(builder =>
                    builder.AddPostgres()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(callerAssembly)
                        .For.Migrations())
                .AddLogging(builder => builder.AddFluentMigratorConsole());

            return services;
        }
    }
}