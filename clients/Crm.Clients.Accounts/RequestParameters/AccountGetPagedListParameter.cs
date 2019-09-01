using System;
using System.Collections.Generic;
using Crm.Clients.Accounts.Models;

namespace Crm.Clients.Accounts.RequestParameters
{
    public class AccountGetPagedListParameter
    {
        public bool? IsLocked { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public ICollection<AccountType>? Types { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}