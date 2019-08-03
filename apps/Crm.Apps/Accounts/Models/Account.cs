using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Accounts.Models
{
    public class Account
    {
        public Account(AccountType type, AccountSetting[] settings = default)
        {
            Id = Guid.NewGuid();
            Type = type;
            IsLocked = false;
            IsDeleted = false;
            CreateDateTime = DateTime.UtcNow;
            Settings = settings ?? new AccountSetting[0];
        }

        [Required]
        public Guid Id { get; }

        [Required]
        public AccountType Type { get; }

        [Required]
        public bool IsLocked { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; }

        public AccountSetting[] Settings { get; set; }
    }
}