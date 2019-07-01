using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.Services;
using Crm.Infrastructure.MessageBroking.Consuming;
using Crm.Infrastructure.MessageBroking.Models;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Products.Consumers
{
    public class ProductStatusesConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IProductStatusesService _productStatusesService;

        public ProductStatusesConsumer(IConsumer consumer, IProductStatusesService productStatusesService)
        {
            _consumer = consumer;
            _productStatusesService = productStatusesService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("ProductStatuses", ActionAsync);

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
            var status = message.Data.FromJsonString<ProductStatus>();

            return _productStatusesService.CreateAsync(message.UserId, status, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newStatus = message.Data.FromJsonString<ProductStatus>();
            if (newStatus.Id.IsEmpty())
            {
                return;
            }

            var oldStatus = await _productStatusesService.GetAsync(newStatus.Id, ct).ConfigureAwait(false);
            if (oldStatus == null)
            {
                return;
            }

            await _productStatusesService.UpdateAsync(message.UserId, oldStatus, newStatus, ct).ConfigureAwait(false);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _productStatusesService.DeleteAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _productStatusesService.RestoreAsync(message.UserId, ids, ct);
        }
    }
}