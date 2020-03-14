using System;
using System.Collections.Generic;
using Crm.Apps.Leads.Models;

namespace Crm.Apps.Leads.v1.Responses
{
    public class LeadAttributeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<LeadAttribute> Attributes { get; set; }
    }
}