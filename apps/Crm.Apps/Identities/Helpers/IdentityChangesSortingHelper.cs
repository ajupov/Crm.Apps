using System.Linq;
using Crm.Apps.Identities.Models;

namespace Crm.Apps.Identities.Helpers
{
    public static class IdentityChangesSortingHelper
    {
        public static IOrderedQueryable<IdentityChange> Sort(this IQueryable<IdentityChange> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(IdentityChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(change => change.Id)
                        : queryable.OrderBy(change => change.Id);
                case nameof(IdentityChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(change => change.CreateDateTime)
                        : queryable.OrderBy(change => change.CreateDateTime);
                default:
                    return queryable.OrderByDescending(change => change.CreateDateTime);
            }
        }
    }
}