using System;
using System.Collections.Generic;
using Crm.Apps.Leads.Models;

namespace Crm.Apps.Leads.V1.Responses
{
    public class LeadSourceGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<LeadSource> Sources { get; set; }
    }
}
