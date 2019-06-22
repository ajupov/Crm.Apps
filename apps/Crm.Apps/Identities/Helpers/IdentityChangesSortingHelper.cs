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
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(IdentityChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}