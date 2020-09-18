using System.Collections.Generic;
using Crm.Apps.Leads.Models;

namespace Crm.Apps.Leads.V1.Responses
{
    public class LeadCommentGetPagedListResponse
    {
        public bool HasCommentsBefore { get; set; }

        public List<LeadComment> Comments { get; set; }
    }
}
