using System.Threading;
using System.Threading.Tasks;
using Crm.Infrastructure.MessageBroking.Consuming;
using Crm.Infrastructure.MessageBroking.Consuming.Configs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Crm.Areas.Accounts.Consumers
{
    public class AccountsConsumer : IHostedService
    {
        private static readonly string[] Topics =
        {
            "accounts"
        };

        private readonly ConsumerConfig _config;
        private IConsumer _consumer;

        public AccountsConsumer(IOptions<ConsumerConfig> options)
        {
            _config = options.Value;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer = new Consumer(_config);

            return _consumer.ConsumeAsync(Topics, ActionAsync);
        }

        public Task StopAsync(CancellationToken ct)
        {
            return Task.CompletedTask;
        }

        private Task ActionAsync(string message)
        {
            return Task.CompletedTask;
        }
    }
}