using System;
using Crm.Apps.Areas.Deals.Models;

namespace Crm.Apps.Areas.Deals.Helpers
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