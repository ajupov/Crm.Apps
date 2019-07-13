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
    public class ActivitiesConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IActivitiesService _dealsService;

        public ActivitiesConsumer(IConsumer consumer, IActivitiesService dealsService)
        {
            _consumer = consumer;
            _dealsService = dealsService;
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
            var deal = message.Data.FromJsonString<Activity>();

            return _dealsService.CreateAsync(message.UserId, deal, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newActivity = message.Data.FromJsonString<Activity>();
            if (newActivity.Id.IsEmpty())
            {
                return;
            }

            var oldActivity = await _dealsService.GetAsync(newActivity.Id, ct);
            if (oldActivity == null)
            {
                return;
            }

            await _dealsService.UpdateAsync(message.UserId, oldActivity, newActivity, ct);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _dealsService.DeleteAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _dealsService.RestoreAsync(message.UserId, ids, ct);
        }
    }
}