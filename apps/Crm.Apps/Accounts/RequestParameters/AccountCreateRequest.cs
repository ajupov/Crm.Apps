using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Crm.Apps.Accounts.Models;

namespace Crm.Apps.Accounts.RequestParameters
{
    public class AccountCreateRequest
    {
        [Required]
        public AccountType Type { get; set; }

        public bool IsLocked { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public List<AccountSetting>? Settings { get; set; } = null;
    }
}