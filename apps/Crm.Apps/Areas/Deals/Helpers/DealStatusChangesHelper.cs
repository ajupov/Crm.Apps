using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Areas.Deals.Models;

namespace Crm.Apps.Areas.Deals.Helpers
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