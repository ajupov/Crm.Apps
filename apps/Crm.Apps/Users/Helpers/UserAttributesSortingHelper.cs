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
                        ? queryable.OrderByDescending(attribute => attribute.Id)
                        : queryable.OrderBy(attribute => attribute.Id);
                case nameof(UserAttribute.Type):
                    return isDesc
                        ? queryable.OrderByDescending(attribute => attribute.Type)
                        : queryable.OrderBy(attribute => attribute.Type);
                case nameof(UserAttribute.Key):
                    return isDesc
                        ? queryable.OrderByDescending(attribute => attribute.Key)
                        : queryable.OrderBy(attribute => attribute.Key);
                case nameof(UserAttribute.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(attribute => attribute.CreateDateTime)
                        : queryable.OrderBy(attribute => attribute.CreateDateTime);
                default:
                    return queryable.OrderByDescending(attribute => attribute.CreateDateTime);
            }
        }
    }
}