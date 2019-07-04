using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.Services;
using Crm.Infrastructure.MessageBroking.Consuming;
using Crm.Infrastructure.MessageBroking.Models;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Deals.Consumers
{
    public class DealsConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IDealsService _dealsService;

        public DealsConsumer(IConsumer consumer, IDealsService dealsService)
        {
            _consumer = consumer;
            _dealsService = dealsService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("Deals", ActionAsync);

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
            var deal = message.Data.FromJsonString<Deal>();

            return _dealsService.CreateAsync(message.UserId, deal, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newDeal = message.Data.FromJsonString<Deal>();
            if (newDeal.Id.IsEmpty())
            {
                return;
            }

            var oldDeal = await _dealsService.GetAsync(newDeal.Id, ct).ConfigureAwait(false);
            if (oldDeal == null)
            {
                return;
            }

            await _dealsService.UpdateAsync(message.UserId, oldDeal, newDeal, ct).ConfigureAwait(false);
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