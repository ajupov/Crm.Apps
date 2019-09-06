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

        public List<AccountType>? Types { get; set; }

        public int Offset { get; set; } = 0;

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; } = "CreateDateTime";

        public string OrderBy { get; set; } = "desc";
    }
}