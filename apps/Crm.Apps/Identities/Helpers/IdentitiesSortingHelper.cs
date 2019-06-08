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
                        ? queryable.OrderByDescending(account => account.Id)
                        : queryable.OrderBy(account => account.Id);     
                case nameof(Identity.Type):
                    return isDesc
                        ? queryable.OrderByDescending(account => account.Type)
                        : queryable.OrderBy(account => account.Type);
                case nameof(Identity.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(account => account.CreateDateTime)
                        : queryable.OrderBy(account => account.CreateDateTime);
                default:
                    return queryable.OrderByDescending(account => account.CreateDateTime);
            }
        }
    }
}