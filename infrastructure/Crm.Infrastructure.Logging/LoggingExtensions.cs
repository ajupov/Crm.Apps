using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Crm.Infrastructure.Logging
{
    public static class LoggingExtensions
    {
        private const string Template = "[{Timestamp:o} - {Level:u3}]: {Message:lj}{NewLine}{Exception}";

        public static IWebHostBuilder ConfigureLogging(this IWebHostBuilder webHostBuilder)
        {
            return webHostBuilder.ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .Enrich.FromLogContext()
                    .WriteTo.Console(outputTemplate: Template)
                    .CreateLogger();

                loggingBuilder.AddSerilog(Log.Logger);
            });
        }
    }
}