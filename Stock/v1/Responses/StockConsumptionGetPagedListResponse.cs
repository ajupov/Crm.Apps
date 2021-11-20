using System;
using System.Collections.Generic;
using Crm.Apps.Stock.Models;

namespace Crm.Apps.Stock.V1.Responses
{
    public class StockConsumptionGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<StockConsumption> Consumptions { get; set; }
    }
}
