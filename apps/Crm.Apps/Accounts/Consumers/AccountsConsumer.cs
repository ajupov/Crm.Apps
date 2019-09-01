using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Accounts.RequestParameters;
using Crm.Apps.Accounts.Services;
using Crm.Infrastructure.MessageBroking.Consuming;
using Crm.Infrastructure.MessageBroking.Models;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Accounts.Consumers
{
    public class AccountsConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IAccountsService _accountsService;

        public AccountsConsumer(IConsumer consumer, IAccountsService accountsService)
        {
            _consumer = consumer;
            _accountsService = accountsService;
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

        private Task ActionAsync(Message message, CancellationToken ct)
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

        private Task CreateAsync(Message message, CancellationToken ct)
        {
            var request = message.Data.FromJsonString<AccountCreateRequest>();

            return _accountsService.CreateAsync(message.UserId, request, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var request = message.Data.FromJsonString<AccountUpdateRequest>();
            if (request.Id.IsEmpty())
            {
                return;
            }

            var account = await _accountsService.GetAsync(request.Id, ct);
            if (account == null)
            {
                return;
            }

            await _accountsService.UpdateAsync(message.UserId, account, request, ct);
        }

        private Task LockAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<Guid[]>();
            if (ids.IsEmpty())
            {
                return Task.CompletedTask;
            }

            return _accountsService.LockAsync(message.UserId, ids, ct);
        }

        private Task UnlockAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<Guid[]>();
            if (ids.IsEmpty())
            {
                return Task.CompletedTask;
            }

            return _accountsService.UnlockAsync(message.UserId, ids, ct);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<Guid[]>();
            if (ids.IsEmpty())
            {
                return Task.CompletedTask;
            }

            return _accountsService.DeleteAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<Guid[]>();
            if (ids.IsEmpty())
            {
                return Task.CompletedTask;
            }

            return _accountsService.RestoreAsync(message.UserId, ids, ct);
        }
    }
}