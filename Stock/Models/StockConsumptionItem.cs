using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Crm.Apps.Stock.Models
{
    public class StockConsumptionItem
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid StockConsumptionId { get; set; }

        public Guid ProductId { get; set; }

        public decimal Count { get; set; }

        public List<Guid> UniqueElementIds { get; set; }
    }
}
