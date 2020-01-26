using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Companies.v1.Models;

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
            var isNew = account.Id.IsEmpty();

            return new CompanyBankAccount
            {
                Id = account.Id,
                CompanyId = companyId,
                Number = account.Number,
                BankNumber = account.BankNumber,
                BankCorrespondentNumber = account.BankCorrespondentNumber,
                BankName = account.BankName,
                IsDeleted = account.IsDeleted,
                CreateDateTime = isNew ? DateTime.UtcNow : account.CreateDateTime,
                ModifyDateTime = isNew ? (DateTime?) null : DateTime.UtcNow
            };
        }
    }
}