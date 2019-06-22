using System.Linq;
using Crm.Apps.Users.Models;

namespace Crm.Apps.Users.Helpers
{
    public static class UserAttributesSortingHelper
    {
        public static IOrderedQueryable<UserAttribute> Sort(this IQueryable<UserAttribute> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(UserAttribute.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(UserAttribute.Type):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Type)
                        : queryable.OrderBy(x => x.Type);
                case nameof(UserAttribute.Key):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Key)
                        : queryable.OrderBy(x => x.Key);
                case nameof(UserAttribute.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}