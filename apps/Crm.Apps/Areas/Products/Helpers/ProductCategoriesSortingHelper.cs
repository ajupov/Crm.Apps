using System.Linq;
using Crm.Apps.Areas.Products.Models;

namespace Crm.Apps.Areas.Products.Helpers
{
    public static class ProductCategoriesSortingHelper
    {
        public static IOrderedQueryable<ProductCategory> Sort(this IQueryable<ProductCategory> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(ProductCategory.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(ProductCategory.Name):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Name)
                        : queryable.OrderBy(x => x.Name);
                case nameof(ProductCategory.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}