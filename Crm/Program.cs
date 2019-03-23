using System.Reflection;
using System.Threading.Tasks;
using Crm.Areas.Accounts.Consumers;
using Crm.Areas.Accounts.Services;
using Crm.Areas.Accounts.Storages;
using Crm.Areas.Accounts.UserContext;
using Crm.Infrastructure.Metrics;
using Crm.Infrastructure.WebApplicationConfiguration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crm
{
    public class Program
    {
        private const string ApplicationName = "Accounts";
        private const string ApplicationVersion = "v1";

        public static Task Main()
        {
            return new WebHostBuilder()
                .ConfigureHost()
                .ConfigureLogging()
                .ConfigureServices((builder, services) =>
                {
                    services.ConfigureConfiguration(builder)
                        .ConfigureUserContext<IUserContext, UserContext>()
                        .AddSingleton<IAccountsService, AccountsService>()
                        .ConfigureMetrics<MetricsCollector, MetricsSettings>(builder, "Metrics")
                        .ConfigureConsumers<AccountsConsumer, AccountsConsumerSettings>(builder, "MbKafka")
                        .ConfigureMigrator(builder, Assembly.GetExecutingAssembly(), "DbSqlMain")
                        .ConfigureOrm<AccountsStorage, AccountsStorageSettings>(builder, "DbSql")
                        .ConfigureApiDocumentation(ApplicationName, ApplicationVersion)
                        .ConfigureMvc();
                })
                .Configure(builder =>
                {
                    builder.ConfigureMiddlewares(ApplicationName, ApplicationVersion);
                    builder.Migrate();
                })
                .Build()
                .RunAsync();
        }
    }
}