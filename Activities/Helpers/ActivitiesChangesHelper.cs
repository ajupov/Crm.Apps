using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Activities.v1.Models;

namespace Crm.Apps.Activities.Helpers
{
    public static class ActivitiesChangesHelper
    {
        public static ActivityChange CreateWithLog(this Activity activity, Guid userId, Action<Activity> action)
        {
            action(activity);

            return new ActivityChange
            {
                ActivityId = activity.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = activity.ToJsonString()
            };
        }

        public static ActivityChange UpdateWithLog(this Activity activity, Guid userId, Action<Activity> action)
        {
            var oldValueJson = activity.ToJsonString();

            action(activity);

            return new ActivityChange
            {
                ActivityId = activity.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = activity.ToJsonString()
            };
        }
    }
}