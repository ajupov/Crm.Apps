using System.Linq;
using Crm.Apps.Users.Models;

namespace Crm.Apps.Users.Helpers
{
    public static class UserAttributesChangesSortingHelper
    {
        public static IOrderedQueryable<UserAttributeChange> Sort(this IQueryable<UserAttributeChange> queryable,
            string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(UserAttributeChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(UserAttributeChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}