using System.Linq;
using Crm.Apps.Products.Models;

namespace Crm.Apps.Products.Helpers
{
    public static class ProductStatusesSortingHelper
    {
        public static IOrderedQueryable<ProductStatus> Sort(this IQueryable<ProductStatus> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(ProductStatus.Id):
                    return isDesc
                        ? queryable.OrderByDescending(group => group.Id)
                        : queryable.OrderBy(group => group.Id);
                case nameof(ProductStatus.Name):
                    return isDesc
                        ? queryable.OrderByDescending(group => group.Name)
                        : queryable.OrderBy(group => group.Name);
                case nameof(ProductStatus.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(group => group.CreateDateTime)
                        : queryable.OrderBy(group => group.CreateDateTime);
                default:
                    return queryable.OrderByDescending(group => group.CreateDateTime);
            }
        }
    }
}