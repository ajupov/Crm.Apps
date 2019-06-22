using System.Linq;
using Crm.Apps.Users.Models;

namespace Crm.Apps.Users.Helpers
{
    public static class UsersGroupChangesSortingHelper
    {
        public static IOrderedQueryable<UserGroupChange> Sort(this IQueryable<UserGroupChange> queryable,
            string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(UserGroupChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(UserGroupChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}