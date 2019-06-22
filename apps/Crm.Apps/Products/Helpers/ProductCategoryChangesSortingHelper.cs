using System.Linq;
using Crm.Apps.Products.Models;

namespace Crm.Apps.Products.Helpers
{
    public static class ProductCategoryChangesSortingHelper
    {
        public static IOrderedQueryable<ProductCategoryChange> Sort(this IQueryable<ProductCategoryChange> queryable,
            string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(ProductCategoryChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(ProductCategoryChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}