using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Products.Models;

namespace Crm.Apps.Products.Mappers
{
    public static class ProductAttributeLinksMapper
    {
        public static List<ProductAttributeLink> Map(this List<ProductAttributeLink> links, Guid productId)
        {
            return links?
                .Select(l => Map(l, productId))
                .ToList();
        }

        public static ProductAttributeLink Map(this ProductAttributeLink link, Guid productId)
        {
            return new ProductAttributeLink
            {
                Id = link.Id,
                ProductId = productId,
                ProductAttributeId = link.ProductAttributeId,
                Value = link.Value
            };
        }
    }
}