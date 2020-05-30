using System.Collections.Generic;
using Crm.Apps.Deals.Models;

namespace Crm.Apps.Deals.V1.Responses
{
    public class DealTypeChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<DealTypeChange> Changes { get; set; }
    }
}
