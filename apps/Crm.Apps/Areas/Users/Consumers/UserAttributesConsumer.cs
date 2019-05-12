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
    public class UserAttributesConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IUserAttributesService _userAttributesService;

        public UserAttributesConsumer(IConsumer consumer, IUserAttributesService userAttributesService)
        {
            _consumer = consumer;
            _userAttributesService = userAttributesService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("UserAttributes", ActionAsync);

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
            var userAttribute = message.Data.FromJsonString<UserAttribute>();
            if (userAttribute.Id == Guid.Empty)
            {
                return Task.CompletedTask;
            }

            return _userAttributesService.CreateAsync(message.UserId, userAttribute, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newUserAttribute = message.Data.FromJsonString<UserAttribute>();
            if (newUserAttribute.Id == Guid.Empty)
            {
                return;
            }

            var oldUserAttribute = await _userAttributesService.GetAsync(newUserAttribute.Id, ct).ConfigureAwait(false);
            if (oldUserAttribute == null)
            {
                return;
            }

            await _userAttributesService.UpdateAsync(message.UserId, oldUserAttribute, newUserAttribute, ct).ConfigureAwait(false);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return Task.CompletedTask;
            }

            return _userAttributesService.RestoreAsync(message.UserId, ids, ct);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return Task.CompletedTask;
            }

            return _userAttributesService.DeleteAsync(message.UserId, ids, ct);
        }
    }
}