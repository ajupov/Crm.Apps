using System.Linq;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.Helpers
{
    public static class ActivitiesChangesSortingHelper
    {
        public static IOrderedQueryable<ActivityChange> Sort(this IQueryable<ActivityChange> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(ActivityChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(ActivityChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}