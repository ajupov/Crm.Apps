using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.Services;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Areas.Leads.Consumers
{
    public class LeadSourcesConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly ILeadSourcesService _leadSourcesService;

        public LeadSourcesConsumer(IConsumer consumer, ILeadSourcesService leadSourcesService)
        {
            _consumer = consumer;
            _leadSourcesService = leadSourcesService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("LeadSources", ActionAsync);

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
            var status = message.Data.FromJsonString<LeadSource>();

            return _leadSourcesService.CreateAsync(message.UserId, status, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newSource = message.Data.FromJsonString<LeadSource>();
            if (newSource.Id.IsEmpty())
            {
                return;
            }

            var oldSource = await _leadSourcesService.GetAsync(newSource.Id, ct);
            if (oldSource == null)
            {
                return;
            }

            await _leadSourcesService.UpdateAsync(message.UserId, oldSource, newSource, ct);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _leadSourcesService.DeleteAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _leadSourcesService.RestoreAsync(message.UserId, ids, ct);
        }
    }
}