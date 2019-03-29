using System.Threading.Tasks;
using Crm.Apps.Areas.Accounts.Consumers;
using Crm.Apps.Areas.Accounts.Services;
using Crm.Apps.Areas.Accounts.Storages;
using Crm.Common.UserContext;
using Crm.Infrastructure.ApiDocumentation;
using Crm.Infrastructure.Configuration;
using Crm.Infrastructure.Host;
using Crm.Infrastructure.Logging;
using Crm.Infrastructure.MessageBroking;
using Crm.Infrastructure.Metrics;
using Crm.Infrastructure.Migrations;
using Crm.Infrastructure.Mvc;
using Crm.Infrastructure.Orm;
using Crm.Infrastructure.Tracing;
using Crm.Infrastructure.UserContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Apps
{
    public static class Program
    {
        private const string ApplicationName = "Crm";
        private const string ApplicationVersion = "v1";

        public static Task Main()
        {
            return
                ConfigurationExtensions.GetConfiguration()
                    .ConfigureHost()
                    .ConfigureLogging()
                    .ConfigureServices((webHostBuilder, services) =>
                    {
                        var configuration = webHostBuilder.Configuration;

                        services
                            .ConfigureApiDocumentation(ApplicationName, ApplicationVersion)
                            .ConfigureMetrics()
                            .ConfigureTracing(ApplicationName)
                            .ConfigureMigrator(configuration)
                            .ConfigureOrm<AccountsStorage>(configuration)
                            .ConfigureConsumer<AccountsConsumer>(configuration)
                            .ConfigureMvc()
                            .ConfigureUserContext<IUserContext, UserContext>()
                            .AddSingleton<IAccountsService, AccountsService>();
                    })
                    .Configure(applicationBuilder => applicationBuilder
                        .UseApiDocumentationsMiddleware(ApplicationName, ApplicationVersion)
                        .UseMigrationsMiddleware()
                        .UseMetricsMiddleware()
                        .UseMvcMiddleware())
                    .Build()
                    .RunAsync();
        }
    }
}