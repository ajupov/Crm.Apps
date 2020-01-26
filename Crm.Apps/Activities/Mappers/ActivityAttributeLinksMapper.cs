using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.Guid;
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
            var isNew = link.Id.IsEmpty();

            return new ActivityAttributeLink
            {
                Id = isNew ? Guid.NewGuid() : link.Id,
                ActivityId = activityId,
                ActivityAttributeId = link.ActivityAttributeId,
                Value = link.Value,
                CreateDateTime = isNew ? DateTime.UtcNow : link.CreateDateTime,
                ModifyDateTime = isNew ? (DateTime?) null : DateTime.UtcNow
            };
        }
    }
}