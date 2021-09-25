using System.Collections.Generic;
using Crm.Apps.Orders.Models;

namespace Crm.Apps.Orders.V1.Responses
{
    public class OrderStatusChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<OrderStatusChange> Changes { get; set; }
    }
}
