using System;
using System.Collections.Generic;

namespace Crm.Apps.Accounts.Models
{
    public class Account
    {
        public Guid Id { get; set; }

        public AccountType Type { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }

        public List<AccountSetting> Settings { get; set; }
    }
}