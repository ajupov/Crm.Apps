using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Accounts.Models
{
    public class Account
    {
        public Account()
        {
        }
        
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

        [Required] public Guid Id { get; set; }

        [Required] public AccountType Type { get; set; }

        [Required] public bool IsLocked { get; set; }

        [Required] public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public List<AccountSetting> Settings { get; set; }
    }
}