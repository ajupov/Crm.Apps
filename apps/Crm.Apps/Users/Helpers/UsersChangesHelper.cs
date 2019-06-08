using System;
using Crm.Apps.Users.Models;
using Crm.Utils.Json;

namespace Crm.Apps.Users.Helpers
{
    public static class UsersChangesHelper
    {
        public static UserChange CreateWithLog(this User user, Guid userId, Action<User> action)
        {
            action(user);

            return new UserChange
            {
                UserId = user.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = user.ToJsonString()
            };
        }

        public static UserChange UpdateWithLog(this User user, Guid userId, Action<User> action)
        {
            var oldValueJson = user.ToJsonString();

            action(user);

            return new UserChange
            {
                UserId = user.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = user.ToJsonString()
            };
        }
    }
}