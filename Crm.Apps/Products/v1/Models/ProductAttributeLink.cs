using System;
using Newtonsoft.Json;

namespace Crm.Apps.Products.v1.Models
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