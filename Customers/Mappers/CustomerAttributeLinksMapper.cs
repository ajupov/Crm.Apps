using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Customers.Models;

namespace Crm.Apps.Customers.Mappers
{
    public static class CustomerAttributeLinksMapper
    {
        public static List<CustomerAttributeLink> Map(this List<CustomerAttributeLink> links, Guid customerId)
        {
            return links?
                .Select(l => Map(l, customerId))
                .ToList();
        }

        public static CustomerAttributeLink Map(this CustomerAttributeLink link, Guid customerId)
        {
            return new CustomerAttributeLink
            {
                Id = link.Id,
                CustomerId = customerId,
                CustomerAttributeId = link.CustomerAttributeId,
                Value = link.Value
            };
        }
    }
}
