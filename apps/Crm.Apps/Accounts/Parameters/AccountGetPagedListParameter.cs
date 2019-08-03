using System;
using Crm.Apps.Accounts.Models;

namespace Crm.Apps.Accounts.Parameters
{
    public class AccountGetPagedListParameter
    {
        public AccountGetPagedListParameter(
            bool? isLocked = default,
            bool? isDeleted = default,
            DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default,
            AccountType[] types = default,
            int offset = default,
            int limit = 10,
            string sortBy = "CreateDateTime",
            string orderBy = "desc")
        {
            IsLocked = isLocked;
            IsDeleted = isDeleted;
            MinCreateDate = minCreateDate;
            MaxCreateDate = maxCreateDate;
            Types = types;
            Offset = offset;
            Limit = limit;
            OrderBy = orderBy;
            SortBy = sortBy;
        }

        public bool? IsLocked { get; }

        public bool? IsDeleted { get; }

        public DateTime? MinCreateDate { get; }

        public DateTime? MaxCreateDate { get; }

        public AccountType[] Types { get; }

        public int Offset { get; }

        public int Limit { get; }

        public string SortBy { get; }

        public string OrderBy { get; }
    }
}