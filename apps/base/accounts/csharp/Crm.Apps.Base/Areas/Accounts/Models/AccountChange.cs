using System;

namespace Crm.Apps.Base.Areas.Accounts.Models
{
    public class AccountChange
    {
        public Guid Id { get; set; }

        public Guid ChangerUserId { get; set; }

        public Guid AccountId { get; set; }

        public DateTime DateTime { get; set; }

        public string OldValueJson { get; set; }

        public string NewValueJson { get; set; }
    }
}