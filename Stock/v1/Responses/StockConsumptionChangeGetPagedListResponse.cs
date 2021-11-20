using System.Collections.Generic;
using Crm.Apps.Stock.Models;

namespace Crm.Apps.Stock.V1.Responses
{
    public class StockConsumptionChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<StockConsumptionChange> Changes { get; set; }
    }
}
