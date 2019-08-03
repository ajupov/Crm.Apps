using System;
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
        private readonly IActivityTypesService _activityTypesService;

        public ActivityTypesConsumer(IConsumer consumer, IActivityTypesService activityTypesService)
        {
            _consumer = consumer;
            _activityTypesService = activityTypesService;
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

            return _activityTypesService.CreateAsync(message.UserId, type, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newType = message.Data.FromJsonString<ActivityType>();
            if (newType.Id.IsEmpty())
            {
                return;
            }

            var oldType = await _activityTypesService.GetAsync(newType.Id, ct);
            if (oldType == null)
            {
                return;
            }

            await _activityTypesService.UpdateAsync(message.UserId, oldType, newType, ct);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<Guid[]>();
            if (ids.IsEmpty())
            {
                return Task.CompletedTask;
            }

            return _activityTypesService.DeleteAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<Guid[]>();
            if (ids.IsEmpty())
            {
                return Task.CompletedTask;
            }

            return _activityTypesService.RestoreAsync(message.UserId, ids, ct);
        }
    }
}