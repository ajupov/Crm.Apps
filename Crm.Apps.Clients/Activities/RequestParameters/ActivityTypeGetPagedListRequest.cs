using System;

namespace Crm.Apps.Clients.Activities.RequestParameters
{
    public class ActivityTypeGetPagedListRequest
    {
        public Guid AccountId { get; set; }

        public string? Name { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; } = 0;

        public int Limit { get; set; } = 10;

        public string? SortBy { get; set; } = "CreateDateTime";

        public string? OrderBy { get; set; } = "desc";
    }
}