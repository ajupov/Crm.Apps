using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Prometheus;

namespace Crm.Infrastructure.Metrics.Collector
{
    public class MetricsCollector : IHostedService
    {
        private readonly KestrelMetricServer _metricsServer;

        public MetricsCollector()
        {
            _metricsServer = new KestrelMetricServer("localhost", 5001);
        }

        public Task StartAsync(CancellationToken ct)
        {
            _metricsServer.Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken ct)
        {
            return _metricsServer.StopAsync();
        }
    }
}