using Microsoft.Extensions.Logging;
using Serilog;

namespace Crm.Infrastructure.Logging
{
    public static class LoggingConfigurationExtensions
    {
        public static void ConfigureLogging(this ILoggingBuilder builder)
        {
            builder.ClearProviders();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate:"[{Timestamp:o} - {Level:u3}]: {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            builder.AddSerilog(Log.Logger);
        }
    }
}