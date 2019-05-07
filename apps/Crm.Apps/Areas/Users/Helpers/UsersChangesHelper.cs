using System;
using System.Collections.Generic;
using Crm.Apps.Areas.Users.Models;
using Crm.Utils.Json;

namespace Crm.Apps.Areas.Users.Helpers
{
    public static class UsersChangesHelper
    {
        public static User CreateWithLog(this User user, Guid userId, Action<User> action)
        {
            action(user);

            var change = new UserChange
            {
                UserId = user.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = user.ToJsonString()
            };

            user.AddChange(change);

            return user;
        }

        public static void UpdateWithLog(this User user, Guid userId, Action<User> action)
        {
            var oldValueJson = user.ToJsonString();

            action(user);

            var change = new UserChange
            {
                UserId = user.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = user.ToJsonString()
            };

            user.AddChange(change);
        }

        private static void AddChange(this User user, UserChange change)
        {
            if (user.Changes == null)
            {
                user.Changes = new List<UserChange>();
            }

            user.Changes.Add(change);
        }
    }
}