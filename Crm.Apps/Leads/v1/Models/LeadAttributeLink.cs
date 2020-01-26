using System;
using Newtonsoft.Json;

namespace Crm.Apps.Leads.v1.Models
{
    public class LeadAttributeLink
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        public Guid LeadId { get; set; }

        public Guid LeadAttributeId { get; set; }

        public string Value { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}