using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.Helpers
{
    public static class ActivityTypeChangesHelper
    {
        public static ActivityTypeChange CreateWithLog(this ActivityType type, Guid userId, Action<ActivityType> action)
        {
            action(type);

            return new ActivityTypeChange
            {
                TypeId = type.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = type.ToJsonString()
            };
        }

        public static ActivityTypeChange UpdateWithLog(this ActivityType type, Guid userId, Action<ActivityType> action)
        {
            var oldValueJson = type.ToJsonString();

            action(type);

            return new ActivityTypeChange
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
