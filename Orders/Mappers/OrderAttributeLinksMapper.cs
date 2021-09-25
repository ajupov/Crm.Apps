using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Orders.Models;

namespace Crm.Apps.Orders.Mappers
{
    public static class OrderAttributeLinksMapper
    {
        public static List<OrderAttributeLink> Map(this List<OrderAttributeLink> links, Guid orderId)
        {
            return links?
                .Select(l => Map(l, orderId))
                .ToList();
        }

        public static OrderAttributeLink Map(this OrderAttributeLink link, Guid orderId)
        {
            return new OrderAttributeLink
            {
                Id = link.Id,
                OrderId = orderId,
                OrderAttributeId = link.OrderAttributeId,
                Value = link.Value
            };
        }
    }
}
