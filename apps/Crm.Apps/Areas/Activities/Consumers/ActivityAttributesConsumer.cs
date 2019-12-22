using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.RequestParameters;
using Crm.Apps.Areas.Activities.Services;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Areas.Activities.Consumers
{
    public class ActivityAttributesConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IActivityAttributesService _activityAttributesService;

        public ActivityAttributesConsumer(IConsumer consumer, IActivityAttributesService activityAttributesService)
        {
            _consumer = consumer;
            _activityAttributesService = activityAttributesService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("ActivityAttributes", ActionAsync);

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
            var request = message.Data.FromJsonString<ActivityAttributeCreateRequest>();

            return _activityAttributesService.CreateAsync(message.UserId, request, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var request = message.Data.FromJsonString<ActivityAttributeUpdateRequest>();
            if (request.Id.IsEmpty())
            {
                return;
            }

            var attribute = await _activityAttributesService.GetAsync(request.Id, ct);
            if (attribute == null)
            {
                return;
            }

            await _activityAttributesService.UpdateAsync(message.UserId, attribute, request, ct);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<Guid[]>();
            if (ids.IsEmpty())
            {
                return Task.CompletedTask;
            }

            return _activityAttributesService.DeleteAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<Guid[]>();
            if (ids.IsEmpty())
            {
                return Task.CompletedTask;
            }

            return _activityAttributesService.RestoreAsync(message.UserId, ids, ct);
        }
    }
}