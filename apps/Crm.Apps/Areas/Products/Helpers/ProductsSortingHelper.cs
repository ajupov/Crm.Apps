using System.Linq;
using Crm.Apps.Areas.Products.Models;

namespace Crm.Apps.Areas.Products.Helpers
{
    public static class ProductsSortingHelper
    {
        public static IOrderedQueryable<Product> Sort(this IQueryable<Product> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(Product.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(Product.Type):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Type)
                        : queryable.OrderBy(x => x.Type);
                case nameof(Product.Name):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Name)
                        : queryable.OrderBy(x => x.Name);
                case nameof(Product.VendorCode):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.VendorCode)
                        : queryable.OrderBy(x => x.VendorCode);
                case nameof(Product.Price):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Price)
                        : queryable.OrderBy(x => x.Price);
                case nameof(Product.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}