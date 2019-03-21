using Crm.Areas.Accounts.Configs;
using Crm.Areas.Accounts.Consumers;
using Crm.Areas.Accounts.Services;
using Crm.Areas.Accounts.Storages;
using Crm.Areas.Accounts.UserContext;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crm.Areas.Accounts.Extensions
{
    public static class ServiceCollectionExtension
    {
        private static IConfiguration _configuration;

        public static IServiceCollection ConfigureAccounts(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            _configuration = configuration;

            services.ConfigureOptions()
                .ConfigureMigrator()
                .ConfigureServices()
                .ConfigureUserContext()
                .ConfigureHostedServices()
                .ConfigureOrm();

            return services;
        }

        private static IServiceCollection ConfigureOptions(this IServiceCollection services)
        {
            services.AddSingleton(_configuration);
            services.AddOptions();

            services.Configure<DbSqlSettings>(_configuration);

            //services.Configure<DbSqlSettings>(settings =>
            //    _configuration.GetSection("DbSqlSettings")
            //        .Bind(settings));

            //services.Configure<MbKafkaSettings>(settings =>
            //    _configuration.GetSection("MbKafkaSettings")
            //        .Bind(settings));

            services.BuildServiceProvider();

            return services;
        }

        private static IServiceCollection ConfigureMigrator(this IServiceCollection services)
        {
            var connectionString = _configuration.Get<DbSqlSettings>().MainConnectionString;

            services.AddFluentMigratorCore()
                .ConfigureRunner(builder =>
                    builder.AddPostgres()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(typeof(object).Assembly)
                        .For
                        .Migrations());

            return services;
        }

        private static IServiceCollection ConfigureOrm(this IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<AccountsStorage>(ServiceLifetime.Singleton)
                .BuildServiceProvider();

            return services;
        }

        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<IAccountsService, AccountsService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }

        private static IServiceCollection ConfigureHostedServices(this IServiceCollection services)
        {
            services.AddSingleton<IHostedService, AccountsConsumer>();

            return services;
        }

        private static IServiceCollection ConfigureUserContext(this IServiceCollection services)
        {
            services.AddScoped<IUserContext, UserContext.UserContext>();

            return services;
        }
    }
}