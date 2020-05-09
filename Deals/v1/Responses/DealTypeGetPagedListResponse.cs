using System;
using System.Collections.Generic;
using Crm.Apps.Deals.Models;

namespace Crm.Apps.Deals.v1.Responses
{
    public class DealTypeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<DealType> Types { get; set; }
    }
}