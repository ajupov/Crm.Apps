using System;
using System.Collections.Generic;
using Crm.Apps.Areas.Users.Models;
using Crm.Utils.Json;

namespace Crm.Apps.Areas.Users.Helpers
{
    public static class UserAttributesChangesHelper
    {
        public static UserAttribute CreateWithLog(this UserAttribute attribute, Guid userId,
            Action<UserAttribute> action)
        {
            action(attribute);

            var change = new UserAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = attribute.ToJsonString()
            };

            attribute.AddChange(change);

            return attribute;
        }

        public static void UpdateWithLog(this UserAttribute attribute, Guid userId, Action<UserAttribute> action)
        {
            var oldValueJson = attribute.ToJsonString();

            action(attribute);

            var change = new UserAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = attribute.ToJsonString()
            };

            attribute.AddChange(change);
        }

        private static void AddChange(this UserAttribute attribute, UserAttributeChange change)
        {
            if (attribute.Changes == null)
            {
                attribute.Changes = new List<UserAttributeChange>();
            }

            attribute.Changes.Add(change);
        }
    }
}