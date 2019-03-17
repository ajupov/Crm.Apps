using System.Threading;
using System.Threading.Tasks;
using Crm.Areas.Accounts.Services;
using Crm.Infrastructure.MessageBroking;
using Crm.Infrastructure.MessageBroking.Consuming;
using Crm.Infrastructure.MessageBroking.Consuming.Configs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Crm.Areas.Accounts.Consumers
{
    public class AccountsConsumer : IHostedService
    {
        private const string AccountsTopic = "accounts";

        private readonly IAccountsService _accountsService;
        private readonly ConsumerConfig _config;
        private IConsumer _consumer;

        public AccountsConsumer(
            IOptions<ConsumerConfig> options,
            IAccountsService accountsService)
        {
            _accountsService = accountsService;
            _config = options.Value;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer = new Consumer(_config);

            return _consumer.ConsumeAsync<object>(ActionAsync, AccountsTopic, ct);
        }

        public Task StopAsync(CancellationToken ct)
        {
            return Task.CompletedTask;
        }

        private Task ActionAsync(Message<object> message, CancellationToken ct)
        {
            switch (message.Type)
            {
                case "create":
                    return _accountsService.CreateAsync(message.UserId, ct);
                case "update":
                    {
                        var 
                        return _accountsService.UpdateAsync(message.UserId, ct);
                    }
            }

            return Task.CompletedTask;
        }
    }
}