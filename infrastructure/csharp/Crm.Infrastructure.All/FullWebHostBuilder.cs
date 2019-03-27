using System;
using System.Reflection;
using System.Threading.Tasks;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crm.Infrastructure.All
{
    public static class FullWebHostBuilder
    {
        public static Task CreateAndRunAsync<TStorage, TConsumer, TUserContext, TUserContextImplementation>(
            this Assembly assembly,
            Action<IServiceCollection> registerApplication, string applicationName, string applicationVersion = "v1")
            where TStorage : DbContext
            where TConsumer : class, IHostedService
            where TUserContext : class
            where TUserContextImplementation : class, TUserContext
        {
            return new WebHostBuilder()
                .ConfigureHost()
                .ConfigureLogging()
                .ConfigureServices((webHostBuilder, services) =>
                {
                    var configuration = webHostBuilder.Configuration;

                    services
                        .ConfigureConfiguration(webHostBuilder)
                        .ConfigureApiDocumentation(applicationName, applicationVersion)
                        .ConfigureMetrics(configuration)
                        .ConfigureTracing(applicationName)
                        .ConfigureMigrator(configuration, assembly)
                        .ConfigureOrm<TStorage>(configuration)
                        .ConfigureConsumer<TConsumer>(configuration)
                        .ConfigureMvc()
                        .ConfigureUserContext<TUserContext, TUserContextImplementation>();

                    registerApplication(services);
                })
                .Configure(applicationBuilder => applicationBuilder
                    .UseApiDocumentationsMiddleware(applicationName, applicationVersion)
                    .UseMigrationsMiddleware()
                    .UseMetricsMiddleware()
                    .UseMvcMiddleware())
                .Build()
                .RunAsync();
        }
    }
}