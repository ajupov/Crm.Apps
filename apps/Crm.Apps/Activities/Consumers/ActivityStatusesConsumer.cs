using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.RequestParameters;
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
            return message.Type switch
            {
                "Create" => CreateAsync(message, ct),
                "Update" => UpdateAsync(message, ct),
                "Delete" => DeleteAsync(message, ct),
                "Restore" => RestoreAsync(message, ct),
                _ => Task.CompletedTask
            };
        }

        private Task CreateAsync(Message message, CancellationToken ct)
        {
            var request = message.Data.FromJsonString<ActivityStatusCreateRequest>();

            return _activityStatusesService.CreateAsync(message.UserId, request, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var request = message.Data.FromJsonString<ActivityStatusUpdateRequest>();
            if (request.Id.IsEmpty())
            {
                return;
            }

            var status = await _activityStatusesService.GetAsync(request.Id, ct);
            if (status == null)
            {
                return;
            }

            await _activityStatusesService.UpdateAsync(message.UserId, status, request, ct);
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