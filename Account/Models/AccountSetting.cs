using System;

namespace Crm.Apps.Account.Models
{
    public class AccountSetting
    {
        [NonDefaultGuid]
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public AccountSettingTaskIndustry? TaskIndustry { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}
