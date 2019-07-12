using System.Linq;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.Helpers
{
    public static class ActivityAttributesChangesSortingHelper
    {
        public static IOrderedQueryable<ActivityAttributeChange> Sort(
            this IQueryable<ActivityAttributeChange> queryable, string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(ActivityAttributeChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(ActivityAttributeChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}