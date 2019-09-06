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
            var request = message.Data.FromJsonString<ActivityTypeCreateRequest>();

            return _activityTypesService.CreateAsync(message.UserId, request, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var request = message.Data.FromJsonString<ActivityTypeUpdateRequest>();
            if (request.Id.IsEmpty())
            {
                return;
            }

            var type = await _activityTypesService.GetAsync(request.Id, ct);
            if (type == null)
            {
                return;
            }

            await _activityTypesService.UpdateAsync(message.UserId, type, request, ct);
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