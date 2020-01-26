using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Companies.v1.Models;

namespace Crm.Apps.Companies.Mappers
{
    public static class CompanyAttributeLinksMapper
    {
        public static List<CompanyAttributeLink> Map(this List<CompanyAttributeLink> links, Guid companyId)
        {
            return links?
                .Select(l => Map(l, companyId))
                .ToList();
        }

        public static CompanyAttributeLink Map(this CompanyAttributeLink link, Guid companyId)
        {
            return new CompanyAttributeLink
            {
                Id = link.Id,
                CompanyId = companyId,
                CompanyAttributeId = link.CompanyAttributeId,
                Value = link.Value
            };
        }
    }
}