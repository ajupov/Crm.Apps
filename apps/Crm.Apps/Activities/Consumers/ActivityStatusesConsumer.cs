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
    public class ActivityStatusesConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IActivityStatusesService _activityStatusesService;

        public ActivityStatusesConsumer(IConsumer consumer, IActivityStatusesService activityStatusesService)
        {
            _consumer = consumer;
            _activityStatusesService = activityStatusesService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("ActivityStatuses", ActionAsync);

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
            var status = message.Data.FromJsonString<ActivityStatus>();

            return _activityStatusesService.CreateAsync(message.UserId, status, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newStatus = message.Data.FromJsonString<ActivityStatus>();
            if (newStatus.Id.IsEmpty())
            {
                return;
            }

            var oldStatus = await _activityStatusesService.GetAsync(newStatus.Id, ct);
            if (oldStatus == null)
            {
                return;
            }

            await _activityStatusesService.UpdateAsync(message.UserId, oldStatus, newStatus, ct);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<Guid[]>();
            if (ids.IsEmpty())
            {
                return Task.CompletedTask;
            }

            return _activityStatusesService.DeleteAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<Guid[]>();
            if (ids.IsEmpty())
            {
                return Task.CompletedTask;
            }

            return _activityStatusesService.RestoreAsync(message.UserId, ids, ct);
        }
    }
}