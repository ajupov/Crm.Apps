using System;
using Crm.Common.All.Validation.Attributes;

namespace Crm.Apps.Account.Models
{
    public class AccountFlag
    {
        [NonDefaultGuid]
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public AccountFlagType Type { get; set; }

        public DateTime SetDateTime { get; set; }
    }
}
