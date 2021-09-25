using System.Collections.Generic;
using Crm.Apps.Orders.Models;

namespace Crm.Apps.Orders.V1.Responses
{
    public class OrderChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<OrderChange> Changes { get; set; }
    }
}
