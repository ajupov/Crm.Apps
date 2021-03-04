using System;

namespace Crm.Apps.Settings.Models
{
    public class UserSetting
    {
        public Guid UserId { get; set; }

        public UserSettingType Type { get; set; }

        public string Value { get; set; }
    }
}
