using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.Services;
using Crm.Infrastructure.MessageBroking.Consuming;
using Crm.Infrastructure.MessageBroking.Models;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Companies.Consumers
{
    public class CompaniesConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly ICompaniesService _companiesService;

        public CompaniesConsumer(IConsumer consumer, ICompaniesService companiesService)
        {
            _consumer = consumer;
            _companiesService = companiesService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("Companies", ActionAsync);

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
            var company = message.Data.FromJsonString<Company>();

            return _companiesService.CreateAsync(message.UserId, company, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newCompany = message.Data.FromJsonString<Company>();
            if (newCompany.Id.IsEmpty())
            {
                return;
            }

            var oldCompany = await _companiesService.GetAsync(newCompany.Id, ct);
            if (oldCompany == null)
            {
                return;
            }

            await _companiesService.UpdateAsync(message.UserId, oldCompany, newCompany, ct);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _companiesService.DeleteAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _companiesService.RestoreAsync(message.UserId, ids, ct);
        }
    }
}