using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Products.Models;
using Crm.Apps.Areas.Products.Services;
using Crm.Infrastructure.MessageBroking.Consuming;
using Crm.Infrastructure.MessageBroking.Models;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Microsoft.Extensions.Hosting;

namespace Crm.Apps.Areas.Products.Consumers
{
    public class ProductsConsumer : IHostedService
    {
        private readonly IConsumer _consumer;
        private readonly IProductsService _productsService;

        public ProductsConsumer(IConsumer consumer, IProductsService productsService)
        {
            _consumer = consumer;
            _productsService = productsService;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Consume("Products", ActionAsync);

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
                case "Hide":
                    return HideAsync(message, ct);
                case "Show":
                    return ShowAsync(message, ct);
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
            var product = message.Data.FromJsonString<Product>();
            if (product.Id.IsEmpty())
            {
                return Task.CompletedTask;
            }

            return _productsService.CreateAsync(message.UserId, product, ct);
        }

        private async Task UpdateAsync(Message message, CancellationToken ct)
        {
            var newProduct = message.Data.FromJsonString<Product>();
            if (newProduct.Id.IsEmpty())
            {
                return;
            }

            var oldProduct = await _productsService.GetAsync(newProduct.Id, ct).ConfigureAwait(false);
            if (oldProduct == null)
            {
                return;
            }

            await _productsService.UpdateAsync(message.UserId, oldProduct, newProduct, ct).ConfigureAwait(false);
        }

        private Task HideAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _productsService.HideAsync(message.UserId, ids, ct);
        }

        private Task ShowAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _productsService.ShowAsync(message.UserId, ids, ct);
        }

        private Task RestoreAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _productsService.RestoreAsync(message.UserId, ids, ct);
        }

        private Task DeleteAsync(Message message, CancellationToken ct)
        {
            var ids = message.Data.FromJsonString<List<Guid>>();
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return Task.CompletedTask;
            }

            return _productsService.DeleteAsync(message.UserId, ids, ct);
        }
    }
}