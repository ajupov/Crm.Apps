using Crm.Areas.Accounts.Settings;
using Crm.Areas.Accounts.Storages;
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
            services.ConfigureOrm();

            return services;
        }

        private static void ConfigureOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var section = configuration.GetSection(nameof(AccountsStorageSettings));

            services.Configure<AccountsStorageSettings>(section);
        }

        private static void ConfigureMigrator(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.Get<AccountsStorageSettings>().ConnectionString;

            services.AddFluentMigratorCore()
                .ConfigureRunner(builder =>
                    builder.AddPostgres()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(typeof(object).Assembly).For.Migrations());
        }

        private static void ConfigureOrm(this IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql()
               .AddDbContext<AccountsStorage>()
               .BuildServiceProvider();
        }
    }
}