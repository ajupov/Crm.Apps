using System;
using System.Collections.Generic;
using Crm.Apps.Orders.Models;

namespace Crm.Apps.Orders.V1.Responses
{
    public class OrderGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<Order> Orders { get; set; }
    }
}
