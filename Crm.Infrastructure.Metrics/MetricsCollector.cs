using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Prometheus;

namespace Crm.Infrastructure.Metrics
{
    public class MetricsCollector : IHostedService
    {
        private readonly KestrelMetricServer _metricsServer;

        public MetricsCollector(IOptions<MetricsSettings> options)
        {
            _metricsServer = new KestrelMetricServer(options.Value.Host, options.Value.Port);
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
