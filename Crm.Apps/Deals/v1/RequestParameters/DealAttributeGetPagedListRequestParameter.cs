using System;
using System.Collections.Generic;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Deals.v1.RequestParameters
{
    public class DealAttributeGetPagedListRequestParameter
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

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}