using System;
using Crm.Apps.Products.Models;
using Crm.Utils.Json;

namespace Crm.Apps.Products.Helpers
{
    public static class ProductsChangesHelper
    {
        public static ProductChange CreateWithLog(this Product product, Guid productId, Action<Product> action)
        {
            action(product);

            return new ProductChange
            {
                ProductId = product.Id,
                ChangerUserId = productId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = product.ToJsonString()
            };
        }

        public static ProductChange UpdateWithLog(this Product product, Guid productId, Action<Product> action)
        {
            var oldValueJson = product.ToJsonString();

            action(product);

            return new ProductChange
            {
                ProductId = product.Id,
                ChangerUserId = productId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = product.ToJsonString()
            };
        }
    }
}