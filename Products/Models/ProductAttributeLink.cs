using System;
using System.Text.Json.Serialization;

namespace Crm.Apps.Products.Models
{
    public class ProductAttributeLink
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid ProductId { get; set; }

        public Guid ProductAttributeId { get; set; }

        public string Value { get; set; }
    }
}
