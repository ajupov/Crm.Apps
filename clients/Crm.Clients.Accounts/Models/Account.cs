using System;
using System.Collections.Generic;

namespace Crm.Clients.Accounts.Models
{
    public class Account
    {
        public Account(
            AccountType type,
            List<AccountSetting> settings = default)
        {
            Id = Guid.NewGuid();
            Type = type;
            IsLocked = false;
            IsDeleted = false;
            CreateDateTime = DateTime.UtcNow;
            Settings = settings ?? new List<AccountSetting>();
        }

        public Guid Id { get; }

        public AccountType Type { get; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; }

        public List<AccountSetting> Settings { get; set; }
    }
}