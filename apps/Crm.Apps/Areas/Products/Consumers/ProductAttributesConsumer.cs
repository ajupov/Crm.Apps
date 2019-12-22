using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Products.Models;
using Crm.Apps.Areas.Products.Services;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Areas.Products.Consumers
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
            var attribute = message.Data.FromJsonString<ProductAttribute>();

            return _productAttributesService.CreateAsync(message.UserId, attribute, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newAttribute = message.Data.FromJsonString<ProductAttribute>();
            if (newAttribute.Id.IsEmpty())
            {
                return;
            }

            var oldAttribute = await _productAttributesService.GetAsync(newAttribute.Id, ct);
            if (oldAttribute == null)
            {
                return;
            }

            await _productAttributesService.UpdateAsync(message.UserId, oldAttribute, newAttribute, ct)
                ;
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

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _productAttributesService.RestoreAsync(message.UserId, ids, ct);
        }
    }
}