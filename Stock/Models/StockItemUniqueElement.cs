using System;
using Newtonsoft.Json;

namespace Crm.Apps.Stock.Models
{
    public class StockItemUniqueElement
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public Guid? CreateUserId { get; set; }

        public Guid ProductId { get; set; }

        public StockItemUniqueElementStatus Status { get; set; }

        public string Value { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}
