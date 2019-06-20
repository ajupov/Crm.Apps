using System.Linq;
using Crm.Apps.Products.Models;

namespace Crm.Apps.Products.Helpers
{
    public static class ProductAttributesSortingHelper
    {
        public static IOrderedQueryable<ProductAttribute> Sort(this IQueryable<ProductAttribute> queryable,
            string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(ProductAttribute.Id):
                    return isDesc
                        ? queryable.OrderByDescending(attribute => attribute.Id)
                        : queryable.OrderBy(attribute => attribute.Id);
                case nameof(ProductAttribute.Type):
                    return isDesc
                        ? queryable.OrderByDescending(attribute => attribute.Type)
                        : queryable.OrderBy(attribute => attribute.Type);
                case nameof(ProductAttribute.Key):
                    return isDesc
                        ? queryable.OrderByDescending(attribute => attribute.Key)
                        : queryable.OrderBy(attribute => attribute.Key);
                case nameof(ProductAttribute.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(attribute => attribute.CreateDateTime)
                        : queryable.OrderBy(attribute => attribute.CreateDateTime);
                default:
                    return queryable.OrderByDescending(attribute => attribute.CreateDateTime);
            }
        }
    }
}