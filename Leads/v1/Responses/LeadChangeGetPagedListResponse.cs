using System.Collections.Generic;
using Crm.Apps.Leads.Models;

namespace Crm.Apps.Leads.v1.Responses
{
    public class LeadChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<LeadChange> Changes { get; set; }
    }
}