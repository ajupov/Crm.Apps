using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.Parameters;
using Crm.Utils.String;

namespace Crm.Apps.Companies.Helpers
{
    public static class CompaniesFiltersHelper
    {
        public static bool FilterByAdditional(this Company company, CompanyGetPagedListParameter parameter)
        {
            return
                (parameter.LeadIds == null || !parameter.LeadIds.Any() ||
                 parameter.LeadIds.Any(x => LeadIdsPredicate(company, x))) &&
                (parameter.CreateUserIds == null || !parameter.CreateUserIds.Any() ||
                 parameter.CreateUserIds.Any(x => CreateUserIdsPredicate(company, x))) &&
                (parameter.ResponsibleUserIds == null || !parameter.ResponsibleUserIds.Any() ||
                 parameter.ResponsibleUserIds.Any(x => ResponsibleUserIdsPredicate(company, x))) &&
                (parameter.Attributes == null || !parameter.Attributes.Any() ||
                 (parameter.AllAttributes is false
                     ? parameter.Attributes.Any(x => AttributePredicate(company, x))
                     : parameter.Attributes.All(x => AttributePredicate(company, x))));
        }

        private static bool LeadIdsPredicate(Company company, Guid id)
        {
            return company.LeadId == id;
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