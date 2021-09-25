using System.Collections.Generic;
using Crm.Apps.Orders.Models;

namespace Crm.Apps.Orders.V1.Responses
{
    public class OrderTypeChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<OrderTypeChange> Changes { get; set; }
    }
}
