using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Settings.Models;

namespace Crm.Apps.Settings.Helpers
{
    public static class AccountSettingsChangesHelper
    {
        public static AccountSettingChange CreateWithLog(
            this AccountSetting setting,
            Guid userId,
            Action<AccountSetting> action)
        {
            action(setting);

            return new AccountSettingChange
            {
                AccountId = setting.AccountId,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = setting.ToJsonString()
            };
        }

        public static AccountSettingChange UpdateWithLog(
            this AccountSetting setting,
            Guid userId,
            Action<AccountSetting> action)
        {
            var oldValueJson = setting.ToJsonString();

            action(setting);

            return new AccountSettingChange
            {
                AccountId = setting.AccountId,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = setting.ToJsonString()
            };
        }
    }
}
