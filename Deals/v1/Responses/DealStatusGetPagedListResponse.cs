using System;
using System.Collections.Generic;
using Crm.Apps.Deals.Models;

namespace Crm.Apps.Deals.V1.Responses
{
    public class DealStatusGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<DealStatus> Statuses { get; set; }
    }
}
