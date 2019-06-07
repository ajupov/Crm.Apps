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
                        ? queryable.OrderByDescending(product => product.Id)
                        : queryable.OrderBy(product => product.Id);
                case nameof(Product.Type):
                    return isDesc
                        ? queryable.OrderByDescending(product => product.Type)
                        : queryable.OrderBy(product => product.Type);
                case nameof(Product.Name):
                    return isDesc
                        ? queryable.OrderByDescending(product => product.Name)
                        : queryable.OrderBy(product => product.Name);
                case nameof(Product.VendorCode):
                    return isDesc
                        ? queryable.OrderByDescending(product => product.VendorCode)
                        : queryable.OrderBy(product => product.VendorCode);
                case nameof(Product.Price):
                    return isDesc
                        ? queryable.OrderByDescending(product => product.Price)
                        : queryable.OrderBy(product => product.Price);
                case nameof(Product.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(product => product.CreateDateTime)
                        : queryable.OrderBy(product => product.CreateDateTime);
                default:
                    return queryable.OrderByDescending(product => product.CreateDateTime);
            }
        }
    }
}