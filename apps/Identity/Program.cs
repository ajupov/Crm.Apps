using System.Threading.Tasks;
using Crm.Infrastructure.ApiDocumentation;
using Crm.Infrastructure.Configuration;
using Crm.Infrastructure.Hosting;
using Crm.Infrastructure.Logging;
using Crm.Infrastructure.Metrics;
using Crm.Infrastructure.Migrations;
using Crm.Infrastructure.Mvc;
using Crm.Infrastructure.Orm;
using Crm.Infrastructure.Tracing;
using Identity.Clients.Storages;
using Identity.Identities.Storages;
using Identity.Users.Storages;
using Microsoft.AspNetCore.Hosting;

namespace Identity
{
    public static class Program
    {
        public static Task Main()
        {
            return
                ConfigurationExtensions.GetConfiguration()
                    .ConfigureHost()
                    .ConfigureLogging()
                    .ConfigureServices((builder, services) => services
                        .ConfigureMvc()
                        .ConfigureTracing()
                        .ConfigureApiDocumentation()
                        .ConfigureMetrics(builder.Configuration)
                        .ConfigureMigrator(builder.Configuration)
                        .ConfigureOrm<UsersStorage>(builder.Configuration)
                        .ConfigureOrm<IdentitiesStorage>(builder.Configuration)
                        .ConfigureOrm<ClientsStorage>(builder.Configuration)
                    )
                    .Configure(builder => builder
                        .UseApiDocumentationsMiddleware()
                        .UseMigrationsMiddleware()
                        .UseMetricsMiddleware()
                        .UseMvcMiddleware())
                    .Build()
                    .RunAsync();
        }
    }
}