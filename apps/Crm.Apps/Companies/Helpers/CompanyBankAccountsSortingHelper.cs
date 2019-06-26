using System.Linq;
using Crm.Apps.Companies.Models;

namespace Crm.Apps.Companies.Helpers
{
    public static class CompanyBankAccountsSortingHelper
    {
        public static IOrderedQueryable<CompanyBankAccount> Sort(this IQueryable<CompanyBankAccount> queryable,
            string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(CompanyBankAccount.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(CompanyBankAccount.Number):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Number)
                        : queryable.OrderBy(x => x.Number);
                case nameof(CompanyBankAccount.BankNumber):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.BankNumber)
                        : queryable.OrderBy(x => x.BankNumber);
                case nameof(CompanyBankAccount.BankCorrespondentNumber):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.BankCorrespondentNumber)
                        : queryable.OrderBy(x => x.BankCorrespondentNumber);
                case nameof(CompanyBankAccount.BankName):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.BankName)
                        : queryable.OrderBy(x => x.BankName);
                case nameof(CompanyBankAccount.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}