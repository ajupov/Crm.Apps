using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.String;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.v1.Requests;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Companies.Helpers
{
    public static class CompaniesFiltersHelper
    {
        public static bool FilterByAdditional(this Company company, CompanyGetPagedListRequest request)
        {
            return (request.BankAccountNumber.IsEmpty() ||
                    company.BankAccounts.Any(x => x.BankNumber == request.BankAccountNumber)) &&
                   (request.BankAccountBankNumber.IsEmpty() ||
                    company.BankAccounts.Any(x => x.BankNumber == request.BankAccountBankNumber)) &&
                   (request.BankAccountBankCorrespondentNumber.IsEmpty() || company.BankAccounts.Any(x =>
                        x.BankCorrespondentNumber == request.BankAccountBankCorrespondentNumber)) &&
                   (request.BankAccountBankName.IsEmpty() ||
                    company.BankAccounts.Any(x =>
                        EF.Functions.Like(x.BankName, $"{request.BankAccountBankName}%"))) &&
                   (request.Types == null || !request.Types.Any() ||
                    request.Types.Any(x => TypesPredicate(company, x))) &&
                   (request.IndustryTypes == null || !request.IndustryTypes.Any() ||
                    request.IndustryTypes.Any(x => IndustryTypesPredicate(company, x))) &&
                   (request.CreateUserIds == null || !request.CreateUserIds.Any() ||
                    request.CreateUserIds.Any(x => CreateUserIdsPredicate(company, x))) &&
                   (request.ResponsibleUserIds == null || !request.ResponsibleUserIds.Any() ||
                    request.ResponsibleUserIds.Any(x => ResponsibleUserIdsPredicate(company, x))) &&
                   (request.Attributes == null || !request.Attributes.Any() ||
                    (request.AllAttributes is false
                        ? request.Attributes.Any(x => AttributePredicate(company, x))
                        : request.Attributes.All(x => AttributePredicate(company, x))));
        }

        private static bool TypesPredicate(Company company, CompanyType type)
        {
            return company.Type == type;
        }

        private static bool IndustryTypesPredicate(Company company, CompanyIndustryType type)
        {
            return company.IndustryType == type;
        }

        private static bool CreateUserIdsPredicate(Company company, Guid id)
        {
            return company.CreateUserId == id;
        }

        private static bool ResponsibleUserIdsPredicate(Company company, Guid id)
        {
            return company.ResponsibleUserId == id;
        }

        private static bool AttributePredicate(Company company, KeyValuePair<Guid, string> pair)
        {
            var (key, value) = pair;

            return company.AttributeLinks != null && company.AttributeLinks.Any(x =>
                       x.CompanyAttributeId == key && (value.IsEmpty() || x.Value == value));
        }
    }
}