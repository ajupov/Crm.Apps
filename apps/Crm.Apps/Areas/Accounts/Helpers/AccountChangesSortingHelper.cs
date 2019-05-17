using System.Linq;
using Crm.Apps.Areas.Accounts.Models;

namespace Crm.Apps.Areas.Accounts.Helpers
{
    public static class AccountChangesSortingHelper
    {
        public static IOrderedQueryable<AccountChange> Sort(this IQueryable<AccountChange> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";
            sortBy = sortBy.Substring(0, 1).ToUpper() + sortBy.Substring(1);

            switch (sortBy)
            {
                case nameof(AccountChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(change => change.Id)
                        : queryable.OrderBy(change => change.Id);
                case nameof(AccountChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(change => change.CreateDateTime)
                        : queryable.OrderBy(change => change.CreateDateTime);
                default:
                    return queryable.OrderByDescending(change => change.CreateDateTime);
            }
        }
    }
}