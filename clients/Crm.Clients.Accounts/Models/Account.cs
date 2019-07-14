using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crm.Clients.Accounts.Models
{
    public class Account
    {
        public Account(
            AccountType type,
            List<AccountSetting> settings = null)
        {
            Id = Guid.NewGuid();
            Type = type;
            IsLocked = false;
            IsDeleted = false;
            CreateDateTime = DateTime.UtcNow;
            Settings = settings ?? new List<AccountSetting>();
        }

        public Guid Id { get; set; }

        public AccountType Type { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public List<AccountSetting> Settings { get; set; }
    }
}