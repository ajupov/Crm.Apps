using System.Collections.Generic;
using Crm.Clients.Accounts.Models;

namespace Crm.Clients.Accounts.RequestParameters
{
    public class AccountCreateRequest
    {
        public AccountType Type { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<AccountSetting>? Settings { get; set; }
    }
}