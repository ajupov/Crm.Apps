using System.Collections.Generic;
using Crm.Apps.Customers.Models;

namespace Crm.Apps.Customers.V1.Responses
{
    public class CustomerSourceChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<CustomerSourceChange> Changes { get; set; }
    }
}
