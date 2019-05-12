using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Services;
using Crm.Infrastructure.MessageBroking.Consuming;
using Crm.Infrastructure.MessageBroking.Models;
using Crm.Utils.Json;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Areas.Users.Consumers
{
    public class UsersConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IUsersService _usersService;

        public UsersConsumer(IConsumer consumer, IUsersService usersService)
        {
            _consumer = consumer;
            _usersService = usersService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("Users", ActionAsync);

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
            var user = message.Data.FromJsonString<User>();
            if (user.Id == Guid.Empty)
            {
                return Task.CompletedTask;
            }

            return _usersService.CreateAsync(message.UserId, user, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newUser = message.Data.FromJsonString<User>();
            if (newUser.Id == Guid.Empty)
            {
                return;
            }

            var oldUser = await _usersService.GetAsync(newUser.Id, ct).ConfigureAwait(false);
            if (oldUser == null)
            {
                return;
            }

            await _usersService.UpdateAsync(message.UserId, oldUser, newUser, ct).ConfigureAwait(false);
        }

        private Task LockAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return Task.CompletedTask;
            }

            return _usersService.LockAsync(message.UserId, ids, ct);
        }

        private Task UnlockAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return Task.CompletedTask;
            }

            return _usersService.UnlockAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return Task.CompletedTask;
            }

            return _usersService.RestoreAsync(message.UserId, ids, ct);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return Task.CompletedTask;
            }

            return _usersService.DeleteAsync(message.UserId, ids, ct);
        }
    }
}