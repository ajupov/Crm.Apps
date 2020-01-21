using System;

namespace Crm.Apps.Deals.v1.Models
{
    public class DealAttributeLink
    {
        public Guid Id { get; set; }

        public Guid DealId { get; set; }

        public Guid DealAttributeId { get; set; }

        public string Value { get; set; }

        public DateTime CreateDateTime { get; set; }
        
        public DateTime? ModifyDateTime { get; set; }
    }
}