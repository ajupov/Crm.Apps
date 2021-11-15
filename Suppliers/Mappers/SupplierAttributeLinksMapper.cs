using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Suppliers.Models;

namespace Crm.Apps.Suppliers.Mappers
{
    public static class SupplierAttributeLinksMapper
    {
        public static List<SupplierAttributeLink> Map(this List<SupplierAttributeLink> links, Guid supplierId)
        {
            return links?
                .Select(l => Map(l, supplierId))
                .ToList();
        }

        public static SupplierAttributeLink Map(this SupplierAttributeLink link, Guid supplierId)
        {
            return new SupplierAttributeLink
            {
                Id = link.Id,
                SupplierId = supplierId,
                SupplierAttributeId = link.SupplierAttributeId,
                Value = link.Value
            };
        }
    }
}
