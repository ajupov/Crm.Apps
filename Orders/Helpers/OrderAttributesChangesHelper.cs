using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Orders.Models;

namespace Crm.Apps.Orders.Helpers
{
    public static class OrderAttributesChangesHelper
    {
        public static OrderAttributeChange CreateWithLog(
            this OrderAttribute attribute,
            Guid userId,
            Action<OrderAttribute> action)
        {
            action(attribute);

            return new OrderAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = attribute.ToJsonString()
            };
        }

        public static OrderAttributeChange UpdateWithLog(
            this OrderAttribute attribute,
            Guid userId,
            Action<OrderAttribute> action)
        {
            var oldValueJson = attribute.ToJsonString();

            action(attribute);

            return new OrderAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = attribute.ToJsonString()
            };
        }
    }
}
