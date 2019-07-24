namespace Crm.Clients.Accounts.Models
{
    public class AccountSetting
    {
        public AccountSetting(
            AccountSettingType type,
            string value = null)
        {
            Type = type;
            Value = value;
        }

        public AccountSettingType Type { get; }

        public string Value { get; }
    }
}