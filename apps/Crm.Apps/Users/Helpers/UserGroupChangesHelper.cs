using System;
using Crm.Apps.Users.Models;
using Crm.Utils.Json;

namespace Crm.Apps.Users.Helpers
{
    public static class UserGroupChangesHelper
    {
        public static UserGroupChange WithCreateLog(this UserGroup group, Guid userId, Action<UserGroup> action)
        {
            action(group);

            return new UserGroupChange
            {
                GroupId = group.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = group.ToJsonString()
            };
        }

        public static UserGroupChange WithUpdateLog(this UserGroup group, Guid userId, Action<UserGroup> action)
        {
            var oldValueJson = group.ToJsonString();

            action(group);

            return new UserGroupChange
            {
                GroupId = group.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = group.ToJsonString()
            };
        }
    }
}