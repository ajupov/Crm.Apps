using System;

namespace Crm.Apps.v1.Clients.Leads.Models
{
    public class LeadAttributeLink
    {
        // public Guid Id { get; set; }
        //
        // public Guid LeadId { get; set; }

        public Guid LeadAttributeId { get; set; }

        public string Value { get; set; }
    }
}