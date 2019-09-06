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
    public class ActivitiesConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IActivitiesService _activitiesService;

        public ActivitiesConsumer(IConsumer consumer, IActivitiesService activitiesService)
        {
            _consumer = consumer;
            _activitiesService = activitiesService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("Activities", ActionAsync);

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
            var request = message.Data.FromJsonString<ActivityCreateRequest>();

            return _activitiesService.CreateAsync(message.UserId, request, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var request = message.Data.FromJsonString<ActivityUpdateRequest>();
            if (request.Id.IsEmpty())
            {
                return;
            }

            var activity = await _activitiesService.GetAsync(request.Id, ct);
            if (activity == null)
            {
                return;
            }

            await _activitiesService.UpdateAsync(message.UserId, activity, request, ct);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<Guid[]>();
            if (ids.IsEmpty())
            {
                return Task.CompletedTask;
            }

            return _activitiesService.DeleteAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<Guid[]>();
            if (ids.IsEmpty())
            {
                return Task.CompletedTask;
            }

            return _activitiesService.RestoreAsync(message.UserId, ids, ct);
        }
    }
}