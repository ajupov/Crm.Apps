using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Companies.Models;

namespace Crm.Apps.Companies.Mappers
{
    public static class CompanyBankAccountsMapper
    {
        public static List<CompanyBankAccount> Map(this List<CompanyBankAccount> accounts, Guid companyId)
        {
            return accounts?
                .Select(l => Map(l, companyId))
                .ToList();
        }

        public static CompanyBankAccount Map(this CompanyBankAccount account, Guid companyId)
        {
            return new CompanyBankAccount
            {
                Id = account.Id,
                CompanyId = companyId,
                Number = account.Number,
                BankNumber = account.BankNumber,
                BankCorrespondentNumber = account.BankCorrespondentNumber,
                BankName = account.BankName,
                IsDeleted = account.IsDeleted
            };
        }
    }
}