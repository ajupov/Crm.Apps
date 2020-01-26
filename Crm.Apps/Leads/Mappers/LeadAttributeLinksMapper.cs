using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Leads.v1.Models;

namespace Crm.Apps.Leads.Mappers
{
    public static class LeadAttributeLinksMapper
    {
        public static List<LeadAttributeLink> Map(this List<LeadAttributeLink> links, Guid leadId)
        {
            return links?
                .Select(l => Map(l, leadId))
                .ToList();
        }

        public static LeadAttributeLink Map(this LeadAttributeLink link, Guid leadId)
        {
            var isNew = link.Id.IsEmpty();

            return new LeadAttributeLink
            {
                Id = link.Id,
                LeadId = leadId,
                LeadAttributeId = link.LeadAttributeId,
                Value = link.Value,
                CreateDateTime = isNew ? DateTime.UtcNow : link.CreateDateTime,
                ModifyDateTime = isNew ? (DateTime?) null : DateTime.UtcNow
            };
        }
    }
}