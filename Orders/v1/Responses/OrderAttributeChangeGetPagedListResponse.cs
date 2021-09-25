using System.Collections.Generic;
using Crm.Apps.Orders.Models;

namespace Crm.Apps.Orders.V1.Responses
{
    public class OrderAttributeChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<OrderAttributeChange> Changes { get; set; }
    }
}
