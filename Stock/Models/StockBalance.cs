using System;
using System.Collections.Generic;
using Crm.Common.All.Validation.Attributes;

namespace Crm.Apps.Stock.Models
{
    public class StockBalance
    {
        [NonDefaultGuid]
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public Guid? CreateUserId { get; set; }

        public Guid RoomId { get; set; }

        public Guid ProductId { get; set; }

        public decimal Count { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }

        // public List<Guid> UniqueElementIds { get; set; }
    }
}
