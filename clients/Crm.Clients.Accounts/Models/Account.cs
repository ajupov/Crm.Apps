using System;
using System.Collections.Generic;

namespace Crm.Clients.Accounts.Models
{
    public class Account
    {
        public Guid Id { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public List<AccountSetting> Settings { get; set; }

        public List<AccountChange> Changes { get; set; }
    }
}