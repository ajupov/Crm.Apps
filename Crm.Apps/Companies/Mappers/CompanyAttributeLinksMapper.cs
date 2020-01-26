using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.Guid;
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
            var isNew = link.Id.IsEmpty();

            return new CompanyAttributeLink
            {
                Id = isNew ? Guid.NewGuid() : link.Id,
                CompanyId = companyId,
                CompanyAttributeId = link.CompanyAttributeId,
                Value = link.Value,
                CreateDateTime = isNew ? DateTime.UtcNow : link.CreateDateTime,
                ModifyDateTime = isNew ? (DateTime?) null : DateTime.UtcNow
            };
        }
    }
}