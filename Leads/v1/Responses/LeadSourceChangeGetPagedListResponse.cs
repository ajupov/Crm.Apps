using System.Collections.Generic;
using Crm.Apps.Leads.Models;

namespace Crm.Apps.Leads.V1.Responses
{
    public class LeadSourceChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<LeadSourceChange> Changes { get; set; }
    }
}
