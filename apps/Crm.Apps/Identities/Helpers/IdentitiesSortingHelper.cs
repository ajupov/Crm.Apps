using System.Linq;
using Crm.Apps.Identities.Models;

namespace Crm.Apps.Identities.Helpers
{
    public static class IdentitiesSortingHelper
    {
        public static IOrderedQueryable<Identity> Sort(this IQueryable<Identity> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(Identity.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);     
                case nameof(Identity.Type):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Type)
                        : queryable.OrderBy(x => x.Type);
                case nameof(Identity.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}