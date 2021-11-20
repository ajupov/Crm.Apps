using System.Collections.Generic;
using Crm.Apps.Stock.Models;

namespace Crm.Apps.Stock.V1.Responses
{
    public class StockBalanceChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<StockBalanceChange> Changes { get; set; }
    }
}
