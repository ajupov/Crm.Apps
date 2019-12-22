using System.Linq;
using Crm.Apps.Areas.Products.Models;

namespace Crm.Apps.Areas.Products.Helpers
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
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(ProductAttributeChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}