using System.Collections.Generic;
using Crm.Apps.Leads.Models;

namespace Crm.Apps.Leads.v1.Responses
{
    public class LeadCommentGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<LeadComment> Comments { get; set; }
    }
}