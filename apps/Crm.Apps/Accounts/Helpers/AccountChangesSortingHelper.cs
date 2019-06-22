using System.Linq;
using Crm.Apps.Accounts.Models;

namespace Crm.Apps.Accounts.Helpers
{
    public static class AccountChangesSortingHelper
    {
        public static IOrderedQueryable<AccountChange> Sort(this IQueryable<AccountChange> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(AccountChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(AccountChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}