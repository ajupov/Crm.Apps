using System;

namespace Crm.Apps.Accounts.RequestParameters
{
    public class AccountChangeGetPagedListRequest
    {
        public AccountChangeGetPagedListRequest(
            Guid accountId,
            Guid? changerUserId = default,
            DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default,
            int offset = default,
            int limit = 10,
            string sortBy = "CreateDateTime",
            string orderBy = "desc")
        {
            AccountId = accountId;
            ChangerUserId = changerUserId;
            MinCreateDate = minCreateDate;
            MaxCreateDate = maxCreateDate;
            Offset = offset;
            Limit = limit;
            OrderBy = orderBy;
            SortBy = sortBy;
        }

        public Guid AccountId { get; }

        public Guid? ChangerUserId { get; }

        public DateTime? MinCreateDate { get; }

        public DateTime? MaxCreateDate { get; }

        public int Offset { get; }

        public int Limit { get; }

        public string SortBy { get; }

        public string OrderBy { get; }
    }
}