﻿using System;
using Crm.Common.All.Validation.Attributes;

namespace Crm.Apps.Stock.Models
{
    public class StockConsumptionChange
    {
        [NonDefaultGuid]
        public Guid Id { get; set; }

        public Guid ChangerUserId { get; set; }

        public Guid StockConsumptionId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string OldValueJson { get; set; }

        public string NewValueJson { get; set; }
    }
}
