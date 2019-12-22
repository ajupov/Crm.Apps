using System.Linq;
using Crm.Apps.Areas.Deals.Models;

namespace Crm.Apps.Areas.Deals.Helpers
{
    public static class DealTypesSortingHelper
    {
        public static IOrderedQueryable<DealType> Sort(this IQueryable<DealType> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(DealType.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(DealType.Name):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Name)
                        : queryable.OrderBy(x => x.Name);
                case nameof(DealType.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}