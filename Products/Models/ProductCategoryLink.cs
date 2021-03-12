using System;
using System.Text.Json.Serialization;

namespace Crm.Apps.Products.Models
{
    public class ProductCategoryLink
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid ProductId { get; set; }

        public Guid ProductCategoryId { get; set; }
    }
}
