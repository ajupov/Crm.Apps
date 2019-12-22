using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.Services;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Areas.Leads.Consumers
{
    public class LeadAttributesConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly ILeadAttributesService _leadAttributesService;

        public LeadAttributesConsumer(IConsumer consumer, ILeadAttributesService leadAttributesService)
        {
            _consumer = consumer;
            _leadAttributesService = leadAttributesService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("LeadAttributes", ActionAsync);

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
            var attribute = message.Data.FromJsonString<LeadAttribute>();

            return _leadAttributesService.CreateAsync(message.UserId, attribute, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newAttribute = message.Data.FromJsonString<LeadAttribute>();
            if (newAttribute.Id.IsEmpty())
            {
                return;
            }

            var oldAttribute = await _leadAttributesService.GetAsync(newAttribute.Id, ct);
            if (oldAttribute == null)
            {
                return;
            }

            await _leadAttributesService.UpdateAsync(message.UserId, oldAttribute, newAttribute, ct)
                ;
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _leadAttributesService.DeleteAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _leadAttributesService.RestoreAsync(message.UserId, ids, ct);
        }
    }
}