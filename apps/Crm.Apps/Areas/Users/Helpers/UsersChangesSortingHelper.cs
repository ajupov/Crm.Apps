using System.Linq;
using Crm.Apps.Areas.Users.Models;

namespace Crm.Apps.Areas.Users.Helpers
{
    public static class UsersChangesSortingHelper
    {
        public static IOrderedQueryable<UserChange> Sort(this IQueryable<UserChange> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(UserChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(change => change.Id)
                        : queryable.OrderBy(change => change.Id);
                case nameof(UserChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(change => change.CreateDateTime)
                        : queryable.OrderBy(change => change.CreateDateTime);
                default:
                    return queryable.OrderByDescending(change => change.CreateDateTime);
            }
        }
    }
}