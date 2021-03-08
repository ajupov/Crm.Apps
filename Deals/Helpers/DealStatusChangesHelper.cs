using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Deals.Models;

namespace Crm.Apps.Deals.Helpers
{
    public static class DealStatusChangesHelper
    {
        public static DealStatusChange CreateWithLog(this DealStatus status, Guid userId, Action<DealStatus> action)
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

        public static DealStatusChange UpdateWithLog(this DealStatus status, Guid userId, Action<DealStatus> action)
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
