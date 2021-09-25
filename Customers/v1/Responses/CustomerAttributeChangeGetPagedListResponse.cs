using System.Collections.Generic;
using Crm.Apps.Customers.Models;

namespace Crm.Apps.Customers.V1.Responses
{
    public class CustomerAttributeChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<CustomerAttributeChange> Changes { get; set; }
    }
}
