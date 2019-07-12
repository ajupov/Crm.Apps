using System;
using Crm.Apps.Activities.Models;
using Crm.Utils.Json;

namespace Crm.Apps.Activities.Helpers
{
    public static class ActivitiesChangesHelper
    {
        public static ActivityChange CreateWithLog(this Activity deal, Guid dealId, Action<Activity> action)
        {
            action(deal);

            return new ActivityChange
            {
                ActivityId = deal.Id,
                ChangerUserId = dealId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = deal.ToJsonString()
            };
        }

        public static ActivityChange UpdateWithLog(this Activity deal, Guid dealId, Action<Activity> action)
        {
            var oldValueJson = deal.ToJsonString();

            action(deal);

            return new ActivityChange
            {
                ActivityId = deal.Id,
                ChangerUserId = dealId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = deal.ToJsonString()
            };
        }
    }
}