using System;

namespace Crm.Apps.Leads.Models
{
    public class LeadAttributeLink
    {
        public Guid Id { get; set; }

        public Guid LeadId { get; set; }

        public Guid LeadAttributeId { get; set; }

        public string Value { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}