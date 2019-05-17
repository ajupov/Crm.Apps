using System;
using Crm.Apps.Areas.Users.Models;
using Crm.Utils.Json;

namespace Crm.Apps.Areas.Users.Helpers
{
    public static class UserAttributesChangesHelper
    {
        public static UserAttributeChange WithCreateLog(this UserAttribute attribute, Guid userId,
            Action<UserAttribute> action)
        {
            action(attribute);

            return new UserAttributeChange
            {
                AttributeId = attribute.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = attribute.ToJsonString()
            };
        }

        public static UserAttributeChange WithUpdateLog(this UserAttribute attribute, Guid userId,
            Action<UserAttribute> action)
        {
            var oldValueJson = attribute.ToJsonString();

            action(attribute);

            return new UserAttributeChange
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