using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.Services;
using Crm.Infrastructure.MessageBroking.Consuming;
using Crm.Infrastructure.MessageBroking.Models;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Activities.Consumers
{
    public class ActivityTypesConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IActivityTypesService _dealTypesService;

        public ActivityTypesConsumer(IConsumer consumer, IActivityTypesService dealTypesService)
        {
            _consumer = consumer;
            _dealTypesService = dealTypesService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("ActivityTypes", ActionAsync);

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
            var type = message.Data.FromJsonString<ActivityType>();

            return _dealTypesService.CreateAsync(message.UserId, type, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newType = message.Data.FromJsonString<ActivityType>();
            if (newType.Id.IsEmpty())
            {
                return;
            }

            var oldType = await _dealTypesService.GetAsync(newType.Id, ct).ConfigureAwait(false);
            if (oldType == null)
            {
                return;
            }

            await _dealTypesService.UpdateAsync(message.UserId, oldType, newType, ct).ConfigureAwait(false);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _dealTypesService.DeleteAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _dealTypesService.RestoreAsync(message.UserId, ids, ct);
        }
    }
}