using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Products.v1.Models;

namespace Crm.Apps.Products.Mappers
{
    public static class ProductCategoryLinksMapper
    {
        public static List<ProductCategoryLink> Map(this List<ProductCategoryLink> links, Guid productId)
        {
            return links?
                .Select(l => Map(l, productId))
                .ToList();
        }

        public static ProductCategoryLink Map(this ProductCategoryLink link, Guid productId)
        {
            var isNew = link.Id.IsEmpty();

            return new ProductCategoryLink
            {
                Id = link.Id,
                ProductId = productId,
                ProductCategoryId = link.ProductCategoryId,
                CreateDateTime = isNew ? DateTime.UtcNow : link.CreateDateTime,
                ModifyDateTime = isNew ? (DateTime?) null : DateTime.UtcNow
            };
        }
    }
}