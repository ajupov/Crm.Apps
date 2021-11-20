using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Crm.Apps.Stock.Models
{
    public class StockArrivalItem
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid StockArrivalId { get; set; }

        public Guid ProductId { get; set; }

        public decimal Count { get; set; }

        public List<Guid> UniqueElementIds { get; set; }
    }
}
