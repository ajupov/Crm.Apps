using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Activities.v1.Models;

namespace Crm.Apps.Activities.Helpers
{
    public static class ActivityStatusChangesHelper
    {
        public static ActivityStatusChange WithCreateLog(
            this ActivityStatus status,
            Guid userId,
            Action<ActivityStatus> action)
        {
            action(status);

            return new ActivityStatusChange
            {
                StatusId = status.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = status.ToJsonString()
            };
        }

        public static ActivityStatusChange WithUpdateLog(
            this ActivityStatus status,
            Guid userId,
            Action<ActivityStatus> action)
        {
            var oldValueJson = status.ToJsonString();

            action(status);

            return new ActivityStatusChange
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