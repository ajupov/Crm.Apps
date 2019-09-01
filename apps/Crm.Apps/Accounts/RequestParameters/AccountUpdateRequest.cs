using System;
using System.ComponentModel.DataAnnotations;
using Crm.Apps.Accounts.Models;

namespace Crm.Apps.Accounts.RequestParameters
{
    public class AccountUpdateRequest
    {
        public AccountUpdateRequest(Guid id, AccountType type, bool isLocked, bool isDeleted, AccountSetting[] settings)
        {
            Id = id;
            Type = type;
            IsLocked = isLocked;
            IsDeleted = isDeleted;
            Settings = settings;
        }

        [Required]
        public Guid Id { get; }

        [Required]
        public AccountType Type { get; }

        [Required]
        public bool IsLocked { get; }

        [Required]
        public bool IsDeleted { get; }

        [Required]
        public AccountSetting[] Settings { get; }
    }
}