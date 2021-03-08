using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Settings.Models;

namespace Crm.Apps.Settings.Helpers
{
    public static class UserSettingsChangesHelper
    {
        public static UserSettingChange CreateWithLog(
            this UserSetting setting,
            Guid userId,
            Action<UserSetting> action)
        {
            action(setting);

            return new UserSettingChange
            {
                UserId = setting.UserId,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = setting.ToJsonString()
            };
        }

        public static UserSettingChange UpdateWithLog(
            this UserSetting setting,
            Guid userId,
            Action<UserSetting> action)
        {
            var oldValueJson = setting.ToJsonString();

            action(setting);

            return new UserSettingChange
            {
                UserId = setting.UserId,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = setting.ToJsonString()
            };
        }
    }
}
