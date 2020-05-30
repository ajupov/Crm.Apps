using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.String;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.V1.Requests;

namespace Crm.Apps.Products.Helpers
{
    public static class ProductsFiltersHelper
    {
        public static bool FilterByAdditional(this Product product, ProductGetPagedListRequest request)
        {
            return (request.Types == null || !request.Types.Any() ||
                    request.Types.Any(x => TypePredicate(product, x))) &&
                   (request.StatusIds == null || !request.StatusIds.Any() ||
                    request.StatusIds.Any(x => StatusIdsPredicate(product, x))) &&
                   (request.Attributes == null || !request.Attributes.Any() ||
                    (request.AllAttributes is false
                        ? request.Attributes.Any(x => AttributePredicate(product, x))
                        : request.Attributes.All(x => AttributePredicate(product, x)))) &&
                   (request.CategoryIds == null || !request.CategoryIds.Any() ||
                    (request.AllCategoryIds is false
                        ? request.CategoryIds.Any(x => CategoryPredicate(product, x))
                        : request.CategoryIds.All(x => CategoryPredicate(product, x))));
        }

        private static bool TypePredicate(Product product, ProductType type)
        {
            return product.Type == type;
        }

        private static bool StatusIdsPredicate(Product product, Guid id)
        {
            return product.StatusId == id;
        }

        private static bool AttributePredicate(Product product, KeyValuePair<Guid, string> pair)
        {
            var (key, value) = pair;

            return product.AttributeLinks != null && product.AttributeLinks.Any(x =>
                       x.ProductAttributeId == key && (value.IsEmpty() || x.Value == value));
        }

        private static bool CategoryPredicate(Product product, Guid id)
        {
            return product.CategoryLinks != null && product.CategoryLinks.Any(x => x.ProductCategoryId == id);
        }
    }
}
