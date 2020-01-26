using System;
using System.Collections.Generic;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Leads.v1.RequestParameters
{
    public class LeadAttributeGetPagedListRequestParameter
    {
        public Guid AccountId { get; set; }

        public List<AttributeType> Types { get; set; }

        public string Key { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public DateTime? MinModifyDate { get; set; }

        public DateTime? MaxModifyDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; } = "CreateDateTime";

        public string OrderBy { get; set; } = "desc";
    }
}