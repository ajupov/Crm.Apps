using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Deals.Models;

namespace Crm.Apps.Deals.Helpers
{
    public static class DealAttributesChangesHelper
    {
        public static DealAttributeChange WithCreateLog(
            this DealAttribute attribute,
            Guid userId,
            Action<DealAttribute> action)
        {
            action(attribute);

            return new DealAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = attribute.ToJsonString()
            };
        }

        public static DealAttributeChange WithUpdateLog(
            this DealAttribute attribute,
            Guid userId,
            Action<DealAttribute> action)
        {
            var oldValueJson = attribute.ToJsonString();

            action(attribute);

            return new DealAttributeChange
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