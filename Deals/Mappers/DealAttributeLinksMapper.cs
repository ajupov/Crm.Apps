using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Deals.Models;

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
            return new DealAttributeLink
            {
                Id = link.Id,
                DealId = dealId,
                DealAttributeId = link.DealAttributeId,
                Value = link.Value
            };
        }
    }
}