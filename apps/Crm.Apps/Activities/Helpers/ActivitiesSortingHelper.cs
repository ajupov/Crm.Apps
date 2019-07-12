using System.Linq;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.Helpers
{
    public static class ActivitiesSortingHelper
    {
        public static IOrderedQueryable<Activity> Sort(this IQueryable<Activity> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(Activity.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(Activity.Name):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Name)
                        : queryable.OrderBy(x => x.Name);
                case nameof(Activity.Priority):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Priority)
                        : queryable.OrderBy(x => x.Priority);
                case nameof(Activity.StartDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.StartDateTime)
                        : queryable.OrderBy(x => x.StartDateTime);
                case nameof(Activity.EndDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.EndDateTime)
                        : queryable.OrderBy(x => x.EndDateTime);
                case nameof(Activity.DeadLineDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.DeadLineDateTime)
                        : queryable.OrderBy(x => x.DeadLineDateTime);
                case nameof(Activity.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}