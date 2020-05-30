using System.Collections.Generic;
using Crm.Apps.Deals.Models;

namespace Crm.Apps.Deals.V1.Responses
{
    public class DealStatusChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<DealStatusChange> Changes { get; set; }
    }
}
