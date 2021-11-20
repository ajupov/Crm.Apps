using System.Collections.Generic;
using Crm.Apps.Stock.Models;

namespace Crm.Apps.Stock.V1.Responses
{
    public class StockArrivalChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<StockArrivalChange> Changes { get; set; }
    }
}
