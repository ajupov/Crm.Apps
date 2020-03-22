using System.Collections.Generic;
using Crm.Apps.Deals.Models;

namespace Crm.Apps.Deals.v1.Responses
{
    public class DealAttributeChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<DealAttributeChange> Changes { get; set; }
    }
}