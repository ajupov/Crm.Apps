using System;
using System.Collections.Generic;
using Crm.Apps.Customers.Models;

namespace Crm.Apps.Customers.V1.Responses
{
    public class CustomerSourceGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<CustomerSource> Sources { get; set; }
    }
}
