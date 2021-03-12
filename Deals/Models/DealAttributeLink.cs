using System;
using System.Text.Json.Serialization;

namespace Crm.Apps.Deals.Models
{
    public class DealAttributeLink
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid DealId { get; set; }

        public Guid DealAttributeId { get; set; }

        public string Value { get; set; }
    }
}
