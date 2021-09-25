using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Orders.Models;

namespace Crm.Apps.Orders.Helpers
{
    public static class OrderTypeChangesHelper
    {
        public static OrderTypeChange CreateWithLog(this OrderType type, Guid userId, Action<OrderType> action)
        {
            action(type);

            return new OrderTypeChange
            {
                TypeId = type.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = type.ToJsonString()
            };
        }

        public static OrderTypeChange UpdateWithLog(this OrderType type, Guid userId, Action<OrderType> action)
        {
            var oldValueJson = type.ToJsonString();

            action(type);

            return new OrderTypeChange
            {
                TypeId = type.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = type.ToJsonString()
            };
        }
    }
}
