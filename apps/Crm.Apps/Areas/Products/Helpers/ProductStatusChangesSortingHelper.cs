using System.Linq;
using Crm.Apps.Areas.Products.Models;

namespace Crm.Apps.Areas.Products.Helpers
{
    public static class ProductStatusChangesSortingHelper
    {
        public static IOrderedQueryable<ProductStatusChange> Sort(this IQueryable<ProductStatusChange> queryable,
            string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(ProductStatusChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(ProductStatusChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}