using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Crm.Apps.Accounts.Models;

namespace Crm.Apps.Accounts.RequestParameters
{
    public class AccountCreateRequest
    {
        [Required]
        public AccountType Type { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public List<AccountSetting>? Settings { get; set; }
    }
}