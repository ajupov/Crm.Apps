using System;
using Newtonsoft.Json;

namespace Crm.Apps.Deals.v1.Models
{
    public class DealPosition
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid DealId { get; set; }

        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductVendorCode { get; set; }

        public decimal Price { get; set; }

        public decimal Count { get; set; }
    }
}