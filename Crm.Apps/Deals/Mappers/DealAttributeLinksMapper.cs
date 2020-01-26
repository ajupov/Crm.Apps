using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Deals.v1.Models;

namespace Crm.Apps.Deals.Mappers
{
    public static class DealAttributeLinksMapper
    {
        public static List<DealAttributeLink> Map(this List<DealAttributeLink> links, Guid dealId)
        {
            return links?
                .Select(l => Map(l, dealId))
                .ToList();
        }

        public static DealAttributeLink Map(this DealAttributeLink link, Guid dealId)
        {
            var isNew = link.Id.IsEmpty();

            return new DealAttributeLink
            {
                Id = link.Id,
                DealId = dealId,
                DealAttributeId = link.DealAttributeId,
                Value = link.Value,
                CreateDateTime = isNew ? DateTime.UtcNow : link.CreateDateTime,
                ModifyDateTime = isNew ? (DateTime?) null : DateTime.UtcNow
            };
        }
    }
}