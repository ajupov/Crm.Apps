using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Crm.Apps.Accounts.Models;

namespace Crm.Apps.Accounts.RequestParameters
{
    public class AccountGetPagedListRequest
    {
        public bool? IsLocked { get; set; }

        public bool? IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? MinCreateDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? MaxCreateDate { get; set; }

        public List<AccountType>? Types { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }

        public string? SortBy { get; set; }

        public string? OrderBy { get; set; }
    }
}