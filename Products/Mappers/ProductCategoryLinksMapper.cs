using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Products.Models;

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
            return new ProductCategoryLink
            {
                Id = link.Id,
                ProductId = productId,
                ProductCategoryId = link.ProductCategoryId
            };
        }
    }
}