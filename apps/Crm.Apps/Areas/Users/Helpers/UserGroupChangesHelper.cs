using System;
using System.Collections.Generic;
using Crm.Apps.Areas.Users.Models;
using Crm.Utils.Json;

namespace Crm.Apps.Areas.Users.Helpers
{
    public static class UserGroupChangesHelper
    {
        public static UserGroup CreateWithLog(this UserGroup group, Guid userId, Action<UserGroup> action)
        {
            action(group);

            var change = new UserGroupChange
            {
                GroupId = group.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = group.ToJsonString()
            };

            group.AddChange(change);

            return group;
        }

        public static void UpdateWithLog(this UserGroup group, Guid userId, Action<UserGroup> action)
        {
            var oldValueJson = group.ToJsonString();

            action(group);

            var change = new UserGroupChange
            {
                GroupId = group.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = group.ToJsonString()
            };

            group.AddChange(change);
        }

        private static void AddChange(this UserGroup group, UserGroupChange change)
        {
            if (group.Changes == null)
            {
                group.Changes = new List<UserGroupChange>();
            }

            group.Changes.Add(change);
        }
    }
}