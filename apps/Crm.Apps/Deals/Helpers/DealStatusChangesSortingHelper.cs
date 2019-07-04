using System.Linq;
using Crm.Apps.Deals.Models;

namespace Crm.Apps.Deals.Helpers
{
    public static class DealTypeChangesSortingHelper
    {
        public static IOrderedQueryable<DealTypeChange> Sort(this IQueryable<DealTypeChange> queryable,
            string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(DealTypeChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(DealTypeChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}