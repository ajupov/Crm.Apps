using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Areas.Accounts.Configs;
using Crm.Areas.Accounts.Models;
using Crm.Areas.Accounts.Services;
using Crm.Infrastructure.MessageBroking;
using Crm.Infrastructure.MessageBroking.Consuming;
using Crm.Infrastructure.MessageBroking.Consuming.Configs;
using Crm.Utils.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Crm.Areas.Accounts.Consumers
{
    public class AccountsConsumer : IHostedService
    {
        private readonly IAccountsService _accountsService;
        private readonly IConsumer _consumer;

        public AccountsConsumer(
            IOptions<MbKafkaSettings> options,
            IAccountsService accountsService)
        {
            var config = new ConsumerConfig
            {
                Host = options.Value.ConnectionString
            };

            _accountsService = accountsService;
            _consumer = new Consumer(config);
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("Accounts", ActionAsync);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken ct)
        {
            _consumer.UnConsume();

            return Task.CompletedTask;
        }

        private Task ActionAsync(
            Message message,
            CancellationToken ct)
        {
            switch (message.Type)
            {
                case "Create":
                    return CreateAsync(message, ct);
                case "Update":
                    return UpdateAsync(message, ct);
                case "Lock":
                    return LockAsync(message, ct);
                case "Unlock":
                    return UnlockAsync(message, ct);
                case "Delete":
                    return DeleteAsync(message, ct);
                case "Restore":
                    return RestoreAsync(message, ct);
                default:
                    return Task.CompletedTask;
            }
        }

        private Task CreateAsync(
            Message message,
            CancellationToken ct)
        {
            return _accountsService.CreateAsync(message.UserId, ct);
        }

        private async Task UpdateAsync(
            Message message,
            CancellationToken ct)
        {
            var newAccount = message.Data
                .FromJsonString<Account>();

            if (newAccount.Id == Guid.Empty)
            {
                return;
            }

            var oldAccount = await _accountsService.GetByIdAsync(newAccount.Id, ct)
                .ConfigureAwait(false);

            if (oldAccount == null)
            {
                return;
            }

            await _accountsService.UpdateAsync(message.UserId, oldAccount, newAccount, ct)
                .ConfigureAwait(false);
        }

        private Task LockAsync(
            Message message,
            CancellationToken ct)
        {
            var ids = message.Data
                .FromJsonString<ICollection<Guid>>();

            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return Task.CompletedTask;
            }

            return _accountsService.LockAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(
            Message message,
            CancellationToken ct)
        {
            var ids = message.Data
                .FromJsonString<ICollection<Guid>>();

            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return Task.CompletedTask;
            }

            return _accountsService.RestoreAsync(message.UserId, ids, ct);
        }

        private Task DeleteAsync(
            Message message,
            CancellationToken ct)
        {
            var ids = message.Data
                .FromJsonString<ICollection<Guid>>();

            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return Task.CompletedTask;
            }

            return _accountsService.DeleteAsync(message.UserId, ids, ct);
        }

        private Task UnlockAsync(
            Message message,
            CancellationToken ct)
        {
            var ids = message.Data
                .FromJsonString<ICollection<Guid>>();

            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return Task.CompletedTask;
            }

            return _accountsService.UnlockAsync(message.UserId, ids, ct);
        }
    }
}