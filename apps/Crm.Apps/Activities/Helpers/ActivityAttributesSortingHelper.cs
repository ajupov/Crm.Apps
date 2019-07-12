using System.Linq;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.Helpers
{
    public static class ActivityAttributesSortingHelper
    {
        public static IOrderedQueryable<ActivityAttribute> Sort(this IQueryable<ActivityAttribute> queryable,
            string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(ActivityAttribute.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(ActivityAttribute.Type):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Type)
                        : queryable.OrderBy(x => x.Type);
                case nameof(ActivityAttribute.Key):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Key)
                        : queryable.OrderBy(x => x.Key);
                case nameof(ActivityAttribute.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}