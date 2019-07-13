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
    public class DealAttributesConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IDealAttributesService _dealAttributesService;

        public DealAttributesConsumer(IConsumer consumer, IDealAttributesService dealAttributesService)
        {
            _consumer = consumer;
            _dealAttributesService = dealAttributesService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("DealAttributes", ActionAsync);

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
            var attribute = message.Data.FromJsonString<DealAttribute>();

            return _dealAttributesService.CreateAsync(message.UserId, attribute, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newAttribute = message.Data.FromJsonString<DealAttribute>();
            if (newAttribute.Id.IsEmpty())
            {
                return;
            }

            var oldAttribute = await _dealAttributesService.GetAsync(newAttribute.Id, ct);
            if (oldAttribute == null)
            {
                return;
            }

            await _dealAttributesService.UpdateAsync(message.UserId, oldAttribute, newAttribute, ct)
                ;
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _dealAttributesService.DeleteAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _dealAttributesService.RestoreAsync(message.UserId, ids, ct);
        }
    }
}