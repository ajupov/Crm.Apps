using System;
using Crm.Apps.Areas.Activities.Models;

namespace Crm.Apps.Areas.Activities.Helpers
{
    public static class ActivityTypeChangesHelper
    {
        public static ActivityTypeChange WithCreateLog(this ActivityType type, Guid userId, Action<ActivityType> action)
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

        public static ActivityTypeChange WithUpdateLog(this ActivityType type, Guid userId, Action<ActivityType> action)
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