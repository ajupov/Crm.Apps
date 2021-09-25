using System.Collections.Generic;
using Crm.Apps.Customers.Models;

namespace Crm.Apps.Customers.V1.Responses
{
    public class CustomerChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<CustomerChange> Changes { get; set; }
    }
}
