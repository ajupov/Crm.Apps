using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Areas.Users.Models;

namespace Crm.Apps.Areas.Users.Helpers
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