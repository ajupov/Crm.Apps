using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Customers.Models;

namespace Crm.Apps.Customers.Helpers
{
    public static class CustomerAttributesChangesHelper
    {
        public static CustomerAttributeChange CreateWithLog(
            this CustomerAttribute attribute,
            Guid userId,
            Action<CustomerAttribute> action)
        {
            action(attribute);

            return new CustomerAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = attribute.ToJsonString()
            };
        }

        public static CustomerAttributeChange UpdateWithLog(
            this CustomerAttribute attribute,
            Guid userId,
            Action<CustomerAttribute> action)
        {
            var oldValueJson = attribute.ToJsonString();

            action(attribute);

            return new CustomerAttributeChange
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
