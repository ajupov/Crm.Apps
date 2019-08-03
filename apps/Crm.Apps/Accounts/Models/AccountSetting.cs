using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Accounts.Models
{
    public class AccountSetting
    {
        public AccountSetting(Guid accountId, AccountSettingType type, string value = default)
        {
            AccountId = accountId;
            Type = type;
            Value = value;
        }

        public Guid AccountId { get; }

        [Required]
        public AccountSettingType Type { get; }

        [Required]
        public string Value { get; }
    }
}