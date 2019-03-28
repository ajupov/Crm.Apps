using System.Linq;
using Crm.Apps.Base.Areas.Accounts.Models;

namespace Crm.Apps.Base.Areas.Accounts.Helpers
{
    public static class AccountsSortingHelper
    {
        public static IOrderedQueryable<Account> Sort(this IQueryable<Account> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(Account.Id):
                    return isDesc
                        ? queryable.OrderByDescending(account => account.Id)
                        : queryable.OrderBy(account => account.Id);
                case nameof(Account.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(account => account.CreateDateTime)
                        : queryable.OrderBy(account => account.CreateDateTime);
                default:
                    return queryable.OrderByDescending(account => account.CreateDateTime);
            }
        }
    }
}