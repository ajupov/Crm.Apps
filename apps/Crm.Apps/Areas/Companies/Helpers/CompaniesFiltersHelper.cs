using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.String;
using Crm.Apps.Areas.Companies.Models;
using Crm.Apps.Areas.Companies.Parameters;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Companies.Helpers
{
    public static class CompaniesFiltersHelper
    {
        public static bool FilterByAdditional(this Company company, CompanyGetPagedListParameter parameter)
        {
            return (parameter.BankAccountNumber.IsEmpty() ||
                    company.BankAccounts.Any(x => x.BankNumber == parameter.BankAccountNumber)) &&
                   (parameter.BankAccountBankNumber.IsEmpty() ||
                    company.BankAccounts.Any(x => x.BankNumber == parameter.BankAccountBankNumber)) &&
                   (parameter.BankAccountBankCorrespondentNumber.IsEmpty() || company.BankAccounts.Any(x =>
                        x.BankCorrespondentNumber == parameter.BankAccountBankCorrespondentNumber)) &&
                   (parameter.BankAccountBankName.IsEmpty() ||
                    company.BankAccounts.Any(x =>
                        EF.Functions.Like(x.BankName, $"{parameter.BankAccountBankName}%"))) &&
                   (parameter.Types == null || !parameter.Types.Any() ||
                    parameter.Types.Any(x => TypesPredicate(company, x))) &&
                   (parameter.IndustryTypes == null || !parameter.IndustryTypes.Any() ||
                    parameter.IndustryTypes.Any(x => IndustryTypesPredicate(company, x))) &&
                   (parameter.CreateUserIds == null || !parameter.CreateUserIds.Any() ||
                    parameter.CreateUserIds.Any(x => CreateUserIdsPredicate(company, x))) &&
                   (parameter.ResponsibleUserIds == null || !parameter.ResponsibleUserIds.Any() ||
                    parameter.ResponsibleUserIds.Any(x => ResponsibleUserIdsPredicate(company, x))) &&
                   (parameter.Attributes == null || !parameter.Attributes.Any() ||
                    (parameter.AllAttributes is false
                        ? parameter.Attributes.Any(x => AttributePredicate(company, x))
                        : parameter.Attributes.All(x => AttributePredicate(company, x))));
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