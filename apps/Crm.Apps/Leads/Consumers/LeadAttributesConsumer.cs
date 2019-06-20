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
    public class ProductAttributesConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IProductAttributesService _productAttributesService;

        public ProductAttributesConsumer(IConsumer consumer, IProductAttributesService productAttributesService)
        {
            _consumer = consumer;
            _productAttributesService = productAttributesService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("ProductAttributes", ActionAsync);

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
            var productAttribute = message.Data.FromJsonString<ProductAttribute>();
            if (productAttribute.Id.IsEmpty())
            {
                return Task.CompletedTask;
            }

            return _productAttributesService.CreateAsync(message.UserId, productAttribute, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newProductAttribute = message.Data.FromJsonString<ProductAttribute>();
            if (newProductAttribute.Id.IsEmpty())
            {
                return;
            }

            var oldProductAttribute =
                await _productAttributesService.GetAsync(newProductAttribute.Id, ct).ConfigureAwait(false);
            if (oldProductAttribute == null)
            {
                return;
            }

            await _productAttributesService.UpdateAsync(message.UserId, oldProductAttribute, newProductAttribute, ct)
                .ConfigureAwait(false);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _productAttributesService.RestoreAsync(message.UserId, ids, ct);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _productAttributesService.DeleteAsync(message.UserId, ids, ct);
        }
    }
}