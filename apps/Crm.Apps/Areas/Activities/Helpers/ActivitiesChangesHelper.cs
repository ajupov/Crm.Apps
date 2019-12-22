using System;
using Crm.Apps.Areas.Activities.Models;

namespace Crm.Apps.Areas.Activities.Helpers
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