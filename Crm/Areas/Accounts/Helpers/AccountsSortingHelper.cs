using System.Linq;
using Crm.Areas.Accounts.Models;

namespace Crm.Areas.Accounts.Helpers
{
    public static class AccountsSortingHelper
    {
        public static IOrderedQueryable<Account> Sort(
            this IQueryable<Account> queryable,
            string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(Account.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(Account.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}