using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Crm.Apps.Accounts.Models;

namespace Crm.Apps.Accounts.RequestParameters
{
    public class AccountGetPagedListRequest
    {
        public bool? IsLocked { get; set; } = false;

        public bool? IsDeleted { get; set; } = false;

        [DataType(DataType.DateTime)]
        public DateTime? MinCreateDate { get; set; } = DateTime.UtcNow.AddDays(-1);

        [DataType(DataType.DateTime)]
        public DateTime? MaxCreateDate { get; set; } = DateTime.UtcNow;

        public List<AccountType>? Types { get; set; } = null;

        public int Offset { get; set; } = 0;

        public int Limit { get; set; } = 10;

        public string? SortBy { get; set; } = "CreateDateTime";

        public string? OrderBy { get; set; } = "desc";
    }
}