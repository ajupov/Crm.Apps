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
                        ? queryable.OrderByDescending(change => change.Id)
                        : queryable.OrderBy(change => change.Id);
                case nameof(ProductStatusChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(change => change.CreateDateTime)
                        : queryable.OrderBy(change => change.CreateDateTime);
                default:
                    return queryable.OrderByDescending(change => change.CreateDateTime);
            }
        }
    }
}