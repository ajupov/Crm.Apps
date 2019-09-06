using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Crm.Apps.Accounts.Models;

namespace Crm.Apps.Accounts.RequestParameters
{
    public class AccountUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public AccountType Type { get; set; }

        [Required]
        public bool IsLocked { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public List<AccountSetting>? Settings { get; set; }
    }
}