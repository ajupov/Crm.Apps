﻿using System;
using System.Collections.Generic;

namespace Crm.Apps.Stock.Models
{
    public class StockArrival
    {
        [NonDefaultGuid]
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public Guid? CreateUserId { get; set; }

        public StockArrivalType Type { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }

        public List<StockArrivalItem> Items { get; set; }
    }
}