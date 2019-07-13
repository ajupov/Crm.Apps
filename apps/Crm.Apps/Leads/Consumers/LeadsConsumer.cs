using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Services;
using Crm.Infrastructure.MessageBroking.Consuming;
using Crm.Infrastructure.MessageBroking.Models;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Leads.Consumers
{
    public class LeadsConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly ILeadsService _leadsService;

        public LeadsConsumer(IConsumer consumer, ILeadsService leadsService)
        {
            _consumer = consumer;
            _leadsService = leadsService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("Leads", ActionAsync);

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
            var lead = message.Data.FromJsonString<Lead>();

            return _leadsService.CreateAsync(message.UserId, lead, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newLead = message.Data.FromJsonString<Lead>();
            if (newLead.Id.IsEmpty())
            {
                return;
            }

            var oldLead = await _leadsService.GetAsync(newLead.Id, ct);
            if (oldLead == null)
            {
                return;
            }

            await _leadsService.UpdateAsync(message.UserId, oldLead, newLead, ct);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _leadsService.DeleteAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _leadsService.RestoreAsync(message.UserId, ids, ct);
        }
    }
}