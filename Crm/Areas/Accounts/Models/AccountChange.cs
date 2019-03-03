using System;
using System.Collections.Generic;
using Crm.Common.Types;

namespace Crm.Areas.Accounts.Models
{
    public class AccountChange
    {
        public Guid Id { get; set; }

        public Guid ChangerUserId { get; set; }

        public Guid AccountId { get; set; }

        public ICollection<ChangeItem> Items { get; set; }

        public DateTime DateTIme { get; set; }
    }
}