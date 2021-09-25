using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Orders.Models;

namespace Crm.Apps.Orders.Helpers
{
    public static class OrderStatusChangesHelper
    {
        public static OrderStatusChange CreateWithLog(this OrderStatus status, Guid userId, Action<OrderStatus> action)
        {
            action(status);

            return new OrderStatusChange
            {
                StatusId = status.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = status.ToJsonString()
            };
        }

        public static OrderStatusChange UpdateWithLog(this OrderStatus status, Guid userId, Action<OrderStatus> action)
        {
            var oldValueJson = status.ToJsonString();

            action(status);

            return new OrderStatusChange
            {
                StatusId = status.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = status.ToJsonString()
            };
        }
    }
}
