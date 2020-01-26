using System;

namespace Crm.Apps.v1.Clients.Deals.Models
{
    public class DealAttributeLink
    {
        // public Guid Id { get; set; }
        //
        // public Guid DealId { get; set; }

        public Guid DealAttributeId { get; set; }

        public string Value { get; set; }
    }
}