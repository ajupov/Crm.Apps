﻿using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.Helpers
{
    public static class ActivityAttributesChangesHelper
    {
        public static ActivityAttributeChange CreateWithLog(
            this ActivityAttribute attribute,
            Guid userId,
            Action<ActivityAttribute> action)
        {
            action(attribute);

            return new ActivityAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = attribute.ToJsonString()
            };
        }

        public static ActivityAttributeChange UpdateWithLog(
            this ActivityAttribute attribute,
            Guid userId,
            Action<ActivityAttribute> action)
        {
            var oldValueJson = attribute.ToJsonString();

            action(attribute);

            return new ActivityAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = attribute.ToJsonString()
            };
        }
    }
}
