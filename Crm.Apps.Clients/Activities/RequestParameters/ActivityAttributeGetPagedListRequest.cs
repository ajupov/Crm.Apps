using System;
using System.Collections.Generic;

namespace Crm.Apps.Clients.Activities.RequestParameters
{
    public class ActivityAttributeGetPagedListRequest
    {
        public Guid AccountId { get; set; }

        public List<AttributeType>? Types { get; set; }

        public string? Key { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; } = 0;

        public int Limit { get; set; } = 10;

        public string? SortBy { get; set; } = "CreateDateTime";

        public string? OrderBy { get; set; } = "desc";
    }
}