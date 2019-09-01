using System;

namespace Crm.Apps.Accounts.Models
{
    public class AccountSetting
    {
        public Guid AccountId { get; set; }

        public AccountSettingType Type { get; set; }

        public string? Value { get; set; }
    }
}