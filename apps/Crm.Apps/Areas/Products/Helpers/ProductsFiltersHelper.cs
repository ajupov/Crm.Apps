using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.String;
using Crm.Apps.Areas.Products.Models;
using Crm.Apps.Areas.Products.Parameters;

namespace Crm.Apps.Areas.Products.Helpers
{
    public static class ProductsFiltersHelper
    {
        public static bool FilterByAdditional(this Product product, ProductGetPagedListParameter parameter)
        {
            return (parameter.Types == null || !parameter.Types.Any() ||
                    parameter.Types.Any(x => TypePredicate(product, x))) &&
                   (parameter.StatusIds == null || !parameter.StatusIds.Any() ||
                    parameter.StatusIds.Any(x => StatusIdsPredicate(product, x))) &&
                   (parameter.Attributes == null || !parameter.Attributes.Any() ||
                    (parameter.AllAttributes is false
                        ? parameter.Attributes.Any(x => AttributePredicate(product, x))
                        : parameter.Attributes.All(x => AttributePredicate(product, x)))) &&
                   (parameter.CategoryIds == null || !parameter.CategoryIds.Any() ||
                    (parameter.AllCategoryIds is false
                        ? parameter.CategoryIds.Any(x => CategoryPredicate(product, x))
                        : parameter.CategoryIds.All(x => CategoryPredicate(product, x))));
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