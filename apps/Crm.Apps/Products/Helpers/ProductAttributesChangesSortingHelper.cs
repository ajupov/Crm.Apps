using System.Linq;
using Crm.Apps.Products.Models;

namespace Crm.Apps.Products.Helpers
{
    public static class ProductAttributesChangesSortingHelper
    {
        public static IOrderedQueryable<ProductAttributeChange> Sort(this IQueryable<ProductAttributeChange> queryable,
            string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(ProductAttributeChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(change => change.Id)
                        : queryable.OrderBy(change => change.Id);
                case nameof(ProductAttributeChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(change => change.CreateDateTime)
                        : queryable.OrderBy(change => change.CreateDateTime);
                default:
                    return queryable.OrderByDescending(change => change.CreateDateTime);
            }
        }
    }
}