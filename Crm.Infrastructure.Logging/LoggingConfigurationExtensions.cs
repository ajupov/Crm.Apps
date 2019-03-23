using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Crm.Infrastructure.Logging
{
    public static class LoggingConfigurationExtensions
    {
        public static IWebHostBuilder ConfigureLogging(this IWebHostBuilder webHostBuilder)
        {
            return webHostBuilder.ConfigureLogging(builder =>
            {
                builder.ClearProviders();

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .WriteTo.Console(outputTemplate: "[{Timestamp:o} - {Level:u3}]: {Message:lj}{NewLine}{Exception}")
                    .CreateLogger();

                builder.AddSerilog(Log.Logger);
            });
        }
    }
}