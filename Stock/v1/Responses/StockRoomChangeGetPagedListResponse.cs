using System.Collections.Generic;
using Crm.Apps.Stock.Models;

namespace Crm.Apps.Stock.V1.Responses
{
    public class StockRoomChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<StockRoomChange> Changes { get; set; }
    }
}
