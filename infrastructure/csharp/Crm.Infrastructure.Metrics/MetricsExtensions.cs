using Crm.Infrastructure.Metrics.Collector;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;

namespace Crm.Infrastructure.Metrics
{
    public static class MetricsExtensions
    {
        public static IServiceCollection ConfigureMetrics(this IServiceCollection services)
        {
            return services
                .AddSingleton<IHostedService, MetricsCollector>();
        }

        public static IApplicationBuilder UseMetricsMiddleware(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseHttpMetrics();
        }
    }
}