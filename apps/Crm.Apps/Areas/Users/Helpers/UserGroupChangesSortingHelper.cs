using System.Linq;
using Crm.Apps.Areas.Users.Models;

namespace Crm.Apps.Areas.Users.Helpers
{
    public static class UsersGroupChangesSortingHelper
    {
        public static IOrderedQueryable<UserGroupChange> Sort(this IQueryable<UserGroupChange> queryable,
            string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";
            sortBy = sortBy.Substring(0, 1).ToUpper() + sortBy.Substring(1);

            switch (sortBy)
            {
                case nameof(UserGroupChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(change => change.Id)
                        : queryable.OrderBy(change => change.Id);
                case nameof(UserGroupChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(change => change.CreateDateTime)
                        : queryable.OrderBy(change => change.CreateDateTime);
                default:
                    return queryable.OrderByDescending(change => change.CreateDateTime);
            }
        }
    }
}