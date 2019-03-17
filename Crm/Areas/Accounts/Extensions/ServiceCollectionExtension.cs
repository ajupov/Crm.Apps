using Crm.Areas.Accounts.Configs;
using Crm.Areas.Accounts.Consumers;
using Crm.Areas.Accounts.Storages;
using Crm.Infrastructure.MessageBroking.Consuming.Configs;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Areas.Accounts.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureAccounts(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.ConfigureOptions(configuration);
            services.ConfigureMigrator(configuration);
            services.ConfigureServices();
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
            services.AddHostedService<AccountsConsumer>();

            return services;
        }

        private static void ConfigureOrm(this IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<AccountsStorage>()
                .BuildServiceProvider();
        }
    }
}