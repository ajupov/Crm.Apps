using System;
using System.Collections.Generic;
using Crm.Apps.Customers.Models;

namespace Crm.Apps.Customers.V1.Responses
{
    public class CustomerAttributeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<CustomerAttribute> Attributes { get; set; }
    }
}
