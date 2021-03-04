using System;

namespace Crm.Apps.Flags.Models
{
    public class AccountFlag
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public AccountFlagType Type { get; set; }

        public DateTime SetDateTime { get; set; }
    }
}
