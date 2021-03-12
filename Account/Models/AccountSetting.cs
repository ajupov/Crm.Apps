using System;

namespace Crm.Apps.Account.Models
{
    public class AccountSetting
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public AccountSettingActivityIndustry? ActivityIndustry { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}
