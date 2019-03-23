using System.Reflection;
using System.Threading.Tasks;
using Crm.Areas.Accounts.Consumers;
using Crm.Areas.Accounts.Services;
using Crm.Areas.Accounts.Storages;
using Crm.Areas.Accounts.UserContext;
using Crm.Infrastructure.WebApplicationConfiguration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crm
{
    public class Program
    {
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
                        .ConfigureConsumers<AccountsConsumer, AccountsConsumerSettings>(builder, "MbKafka")
                        .ConfigureMigrator(builder, Assembly.GetExecutingAssembly(), "DbSqlMain")
                        .ConfigureOrm<AccountsStorage, AccountsStorageSettings>(builder, "DbSql")
                        .ConfigureMvc();
                })
                .Configure(builder =>
                {
                    builder.ConfigureMiddlewares();
                    builder.Migrate();
                })
                .Build()
                .RunAsync();
        }
    }
}