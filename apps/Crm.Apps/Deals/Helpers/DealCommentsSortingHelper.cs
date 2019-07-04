using System.Linq;
using Crm.Apps.Deals.Models;

namespace Crm.Apps.Deals.Helpers
{
    public static class DealCommentsSortingHelper
    {
        public static IOrderedQueryable<DealComment> Sort(this IQueryable<DealComment> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(DealComment.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(DealComment.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}