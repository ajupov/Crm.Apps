using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Companies.Models;
using Crm.Apps.Areas.Companies.Services;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Areas.Companies.Consumers
{
    public class CompanyAttributesConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly ICompanyAttributesService _companyAttributesService;

        public CompanyAttributesConsumer(IConsumer consumer, ICompanyAttributesService companyAttributesService)
        {
            _consumer = consumer;
            _companyAttributesService = companyAttributesService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("CompanyAttributes", ActionAsync);

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
            var attribute = message.Data.FromJsonString<CompanyAttribute>();

            return _companyAttributesService.CreateAsync(message.UserId, attribute, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newAttribute = message.Data.FromJsonString<CompanyAttribute>();
            if (newAttribute.Id.IsEmpty())
            {
                return;
            }

            var oldAttribute = await _companyAttributesService.GetAsync(newAttribute.Id, ct);
            if (oldAttribute == null)
            {
                return;
            }

            await _companyAttributesService.UpdateAsync(message.UserId, oldAttribute, newAttribute, ct)
                ;
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _companyAttributesService.DeleteAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _companyAttributesService.RestoreAsync(message.UserId, ids, ct);
        }
    }
}