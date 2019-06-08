using System.Linq;
using Crm.Apps.Products.Models;

namespace Crm.Apps.Products.Helpers
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
                        ? queryable.OrderByDescending(category => category.Id)
                        : queryable.OrderBy(category => category.Id);
                case nameof(ProductCategory.Name):
                    return isDesc
                        ? queryable.OrderByDescending(category => category.Name)
                        : queryable.OrderBy(category => category.Name);
                case nameof(ProductCategory.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(category => category.CreateDateTime)
                        : queryable.OrderBy(category => category.CreateDateTime);
                default:
                    return queryable.OrderByDescending(category => category.CreateDateTime);
            }
        }
    }
}