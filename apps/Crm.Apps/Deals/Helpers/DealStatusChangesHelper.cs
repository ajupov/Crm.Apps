using System;
using Crm.Apps.Deals.Models;
using Crm.Utils.Json;

namespace Crm.Apps.Deals.Helpers
{
    public static class DealStatusChangesHelper
    {
        public static DealStatusChange WithCreateLog(this DealStatus status, Guid userId, Action<DealStatus> action)
        {
            action(status);

            return new DealStatusChange
            {
                StatusId = status.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = status.ToJsonString()
            };
        }

        public static DealStatusChange WithUpdateLog(this DealStatus status, Guid userId, Action<DealStatus> action)
        {
            var oldValueJson = status.ToJsonString();

            action(status);

            return new DealStatusChange
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