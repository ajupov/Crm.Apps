using System;
using System.IO;
using System.Threading.Tasks;
using Crm.Areas.Accounts.Extensions;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Crm
{
    public class Program
    {
        public static Task Main()
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration(builder =>
                {
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    builder.SetBasePath(Environment.CurrentDirectory)
                        .AddJsonFile($"appsettings.json", false)
                        .AddEnvironmentVariables();

                    builder.Build();
                })
                .ConfigureLogging(builder =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel
                        .Debug()
                        .Enrich
                        .FromLogContext()
                        .WriteTo
                        .Console(outputTemplate:
                            "[{Timestamp:o} - {Level:u3}]: {Message:lj}{NewLine}{Exception}")
                        .CreateLogger();

                    builder.ClearProviders();
                    builder.AddSerilog(Log.Logger);
                })
                .ConfigureServices((builder, services) =>
                {
                    services.ConfigureAccounts(builder.Configuration);

                    services.AddMvc()
                        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
                })
                .Configure(builder =>
                {
                    using (var scope = builder.ApplicationServices
                        .GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        scope.ServiceProvider
                            .GetService<IMigrationRunner>()
                            .MigrateUp();
                    }

                    builder.UseHttpsRedirection();
                    builder.UseMvc();
                })
                .Build()
                .RunAsync();
        }
    }
}