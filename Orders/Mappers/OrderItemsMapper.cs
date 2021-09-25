using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Orders.Models;

namespace Crm.Apps.Orders.Mappers
{
    public static class OrderItemsMapper
    {
        public static List<OrderItem> Map(this List<OrderItem> items, Guid orderId)
        {
            return items?
                .Select(l => Map(l, orderId))
                .ToList();
        }

        public static OrderItem Map(this OrderItem item, Guid orderId)
        {
            return new OrderItem
            {
                Id = item.Id,
                OrderId = orderId,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ProductVendorCode = item.ProductVendorCode,
                Price = item.Price,
                Count = item.Count
            };
        }
    }
}
