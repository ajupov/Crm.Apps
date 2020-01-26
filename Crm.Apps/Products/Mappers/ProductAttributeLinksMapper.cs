using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Products.v1.Models;

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
            var isNew = link.Id.IsEmpty();

            return new ProductAttributeLink
            {
                Id = isNew ? Guid.NewGuid() : link.Id,
                ProductId = productId,
                ProductAttributeId = link.ProductAttributeId,
                Value = link.Value,
                CreateDateTime = isNew ? DateTime.UtcNow : link.CreateDateTime,
                ModifyDateTime = isNew ? (DateTime?) null : DateTime.UtcNow
            };
        }
    }
}