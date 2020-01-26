using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Activities.v1.Models;

namespace Crm.Apps.Activities.Mappers
{
    public static class ActivityAttributeLinksMapper
    {
        public static List<ActivityAttributeLink> Map(this List<ActivityAttributeLink> links, Guid activityId)
        {
            return links?
                .Select(l => Map(l, activityId))
                .ToList();
        }

        public static ActivityAttributeLink Map(this ActivityAttributeLink link, Guid activityId)
        {
            return new ActivityAttributeLink
            {
                Id = link.Id,
                ActivityId = activityId,
                ActivityAttributeId = link.ActivityAttributeId,
                Value = link.Value
            };
        }
    }
}