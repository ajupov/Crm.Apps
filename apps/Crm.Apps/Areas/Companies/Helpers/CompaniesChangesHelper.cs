using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Areas.Companies.Models;

namespace Crm.Apps.Areas.Companies.Helpers
{
    public static class CompaniesChangesHelper
    {
        public static CompanyChange CreateWithLog(this Company company, Guid productId, Action<Company> action)
        {
            action(company);

            return new CompanyChange
            {
                CompanyId = company.Id,
                ChangerUserId = productId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = company.ToJsonString()
            };
        }

        public static CompanyChange UpdateWithLog(this Company company, Guid productId, Action<Company> action)
        {
            var oldValueJson = company.ToJsonString();

            action(company);

            return new CompanyChange
            {
                CompanyId = company.Id,
                ChangerUserId = productId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = company.ToJsonString()
            };
        }
    }
}