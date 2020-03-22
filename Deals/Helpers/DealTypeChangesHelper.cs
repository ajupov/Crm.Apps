using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Deals.Models;

namespace Crm.Apps.Deals.Helpers
{
    public static class DealTypeChangesHelper
    {
        public static DealTypeChange WithCreateLog(this DealType type, Guid userId, Action<DealType> action)
        {
            action(type);

            return new DealTypeChange
            {
                TypeId = type.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = type.ToJsonString()
            };
        }

        public static DealTypeChange WithUpdateLog(this DealType type, Guid userId, Action<DealType> action)
        {
            var oldValueJson = type.ToJsonString();

            action(type);

            return new DealTypeChange
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