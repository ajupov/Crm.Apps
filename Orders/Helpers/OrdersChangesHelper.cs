using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Orders.Models;

namespace Crm.Apps.Orders.Helpers
{
    public static class OrdersChangesHelper
    {
        public static OrderChange CreateWithLog(this Order order, Guid userId, Action<Order> action)
        {
            action(order);

            return new OrderChange
            {
                OrderId = order.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = order.ToJsonString()
            };
        }

        public static OrderChange UpdateWithLog(this Order order, Guid userId, Action<Order> action)
        {
            var oldValueJson = order.ToJsonString();

            action(order);

            return new OrderChange
            {
                OrderId = order.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = order.ToJsonString()
            };
        }
    }
}
