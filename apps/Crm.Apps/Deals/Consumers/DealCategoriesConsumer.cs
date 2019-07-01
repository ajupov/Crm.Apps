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
    public class ProductCategoriesConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IProductCategoriesService _productCategoriesService;

        public ProductCategoriesConsumer(IConsumer consumer, IProductCategoriesService productCategoriesService)
        {
            _consumer = consumer;
            _productCategoriesService = productCategoriesService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("ProductCategories", ActionAsync);

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
            var productCategory = message.Data.FromJsonString<ProductCategory>();

            return _productCategoriesService.CreateAsync(message.UserId, productCategory, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newCategory = message.Data.FromJsonString<ProductCategory>();
            if (newCategory.Id.IsEmpty())
            {
                return;
            }

            var oldCategory = await _productCategoriesService.GetAsync(newCategory.Id, ct).ConfigureAwait(false);
            if (oldCategory == null)
            {
                return;
            }

            await _productCategoriesService.UpdateAsync(message.UserId, oldCategory, newCategory, ct)
                .ConfigureAwait(false);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _productCategoriesService.DeleteAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _productCategoriesService.RestoreAsync(message.UserId, ids, ct);
        }
    }
}