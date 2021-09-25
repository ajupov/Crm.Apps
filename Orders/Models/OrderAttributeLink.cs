using System;
using System.Text.Json.Serialization;

namespace Crm.Apps.Orders.Models
{
    public class OrderAttributeLink
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid OrderId { get; set; }

        public Guid OrderAttributeId { get; set; }

        public string Value { get; set; }
    }
}
