using System.Linq;
using Crm.Apps.Users.Models;

namespace Crm.Apps.Users.Helpers
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
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(UserGroup.Name):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Name)
                        : queryable.OrderBy(x => x.Name);
                case nameof(UserGroup.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}