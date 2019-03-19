using Crm.Areas.Accounts.Configs;
using Crm.Areas.Accounts.Consumers;
using Crm.Areas.Accounts.Services;
using Crm.Areas.Accounts.Storages;
using Crm.Areas.Accounts.UserContext;
using Crm.Infrastructure.MessageBroking.Consuming.Configs;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Crm.Areas.Accounts.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IWebHostBuilder ConfigureSerilog(this IWebHostBuilder builder)
        {
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .WriteTo.Console(outputTemplate:
                        "[{Timestamp:o} - {Level:u3}]: {Message:lj}{NewLine}{Exception}")
                    .CreateLogger();

                logging.AddSerilog(Log.Logger);
            });

            return builder;
        }

        public static IServiceCollection ConfigureAccounts(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.ConfigureOptions(configuration);
            services.ConfigureMigrator(configuration);
            services.ConfigureServices();
            services.ConfigureHostedServices();
            services.ConfigureOrm();

            return services;
        }

        private static void ConfigureOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var accountsStorageConfig = configuration.GetSection(nameof(AccountsStorageConfig));
            services.Configure<AccountsStorageConfig>(accountsStorageConfig);

            var consumerConfig = configuration.GetSection(nameof(ConsumerConfig));
            services.Configure<ConsumerConfig>(consumerConfig);
        }

        private static void ConfigureMigrator(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.Get<AccountsStorageConfig>().ConnectionString;

            services.AddFluentMigratorCore()
                .ConfigureRunner(builder =>
                    builder.AddPostgres()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(typeof(object).Assembly).For.Migrations());
        }

        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<IAccountsService, AccountsService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserContext, UserContext.UserContext>();

            return services;
        }

        private static IServiceCollection ConfigureHostedServices(this IServiceCollection services)
        {
            services.AddSingleton<IHostedService, AccountsConsumer>();

            return services;
        }

        private static void ConfigureOrm(this IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<AccountsStorage>(ServiceLifetime.Singleton)
                .BuildServiceProvider();
        }
    }
}