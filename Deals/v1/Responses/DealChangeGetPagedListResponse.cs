using System.Collections.Generic;
using Crm.Apps.Deals.Models;

namespace Crm.Apps.Deals.v1.Responses
{
    public class DealChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<DealChange> Changes { get; set; }
    }
}