using System;
using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Crm.Infrastructure.WebApplicationConfiguration
{
    public static class WebApplicationConfigurationExtensions
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

        public static IWebHostBuilder ConfigureLogging(this IWebHostBuilder webHostBuilder)
        {
            return webHostBuilder.ConfigureLogging(builder =>
            {
                builder.ClearProviders();

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .WriteTo.Console(outputTemplate: "[{Timestamp:o} - {Level:u3}]: {Message:lj}{NewLine}{Exception}")
                    .CreateLogger();

                builder.AddSerilog(Log.Logger);
            });
        }

        public static IServiceCollection ConfigureConfiguration(this IServiceCollection services,
            WebHostBuilderContext webHostBuilder)
        {
            services.AddSingleton(webHostBuilder.Configuration);
            services.AddOptions();

            return services;
        }

        public static IServiceCollection ConfigureUserContext<TUserContext, TUserContextImplementation>(
            this IServiceCollection services)
                where TUserContext : class
                where TUserContextImplementation : class, TUserContext
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<TUserContext, TUserContextImplementation>();

            return services;
        }

        public static IServiceCollection ConfigureConsumers<TConsumer, TSettings>(this IServiceCollection services,
            WebHostBuilderContext webHostBuilder, string settingsKey)
                where TConsumer : class, IHostedService
                where TSettings : class
        {
            var settings = webHostBuilder.Configuration.GetSection(settingsKey);

            services.Configure<TSettings>(settings);
            services.AddSingleton<IHostedService, TConsumer>();

            return services;
        }

        public static IServiceCollection ConfigureMigrator(this IServiceCollection services,
            WebHostBuilderContext webHostBuilder, Assembly callerAssembly, string connectionStringKey)
        {
            var connectionString = webHostBuilder.Configuration.GetConnectionString(connectionStringKey);

            services.AddFluentMigratorCore().ConfigureRunner(builder =>
                    builder.AddPostgres()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(callerAssembly)
                        .For.Migrations())
                .AddLogging(builder => builder.AddFluentMigratorConsole());

            return services;
        }

        public static void Migrate(this IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                scope.ServiceProvider.GetService<IMigrationRunner>().MigrateUp();
            }
        }

        public static IServiceCollection ConfigureOrm<TStorage, TSettings>(this IServiceCollection services,
            WebHostBuilderContext webHostBuilder, string settingsKey)
                where TStorage : DbContext
                where TSettings : class
        {
            var settings = webHostBuilder.Configuration.GetSection(settingsKey);

            services.Configure<TSettings>(settings);
            services.AddEntityFrameworkNpgsql().AddDbContext<TStorage>(ServiceLifetime.Singleton)
                .BuildServiceProvider();

            return services;
        }

        public static IServiceCollection ConfigureMvc(this IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return services;
        }

        public static IApplicationBuilder ConfigureMiddlewares(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMvc();

            return applicationBuilder;
        }
    }
}