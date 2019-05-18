using System.Linq;
using Crm.Apps.Areas.Users.Models;

namespace Crm.Apps.Areas.Users.Helpers
{
    public static class UserGroupsSortingHelper
    {
        public static IOrderedQueryable<UserGroup> Sort(this IQueryable<UserGroup> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(UserGroup.Id):
                    return isDesc
                        ? queryable.OrderByDescending(group => group.Id)
                        : queryable.OrderBy(group => group.Id);
                case nameof(UserGroup.Name):
                    return isDesc
                        ? queryable.OrderByDescending(group => group.Name)
                        : queryable.OrderBy(group => group.Name);
                case nameof(UserGroup.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(group => group.CreateDateTime)
                        : queryable.OrderBy(group => group.CreateDateTime);
                default:
                    return queryable.OrderByDescending(group => group.CreateDateTime);
            }
        }
    }
}