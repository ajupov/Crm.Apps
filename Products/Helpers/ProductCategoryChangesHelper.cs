using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Products.Models;

namespace Crm.Apps.Products.Helpers
{
    public static class ProductCategoryChangesHelper
    {
        public static ProductCategoryChange CreateWithLog(
            this ProductCategory category,
            Guid userId,
            Action<ProductCategory> action)
        {
            action(category);

            return new ProductCategoryChange
            {
                CategoryId = category.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = category.ToJsonString()
            };
        }

        public static ProductCategoryChange UpdateWithLog(
            this ProductCategory category,
            Guid userId,
            Action<ProductCategory> action)
        {
            var oldValueJson = category.ToJsonString();

            action(category);

            return new ProductCategoryChange
            {
                CategoryId = category.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = category.ToJsonString()
            };
        }
    }
}
