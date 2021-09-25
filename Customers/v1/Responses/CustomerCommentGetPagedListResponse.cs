using System.Collections.Generic;
using Crm.Apps.Customers.Models;

namespace Crm.Apps.Customers.V1.Responses
{
    public class CustomerCommentGetPagedListResponse
    {
        public bool HasCommentsBefore { get; set; }

        public List<CustomerComment> Comments { get; set; }
    }
}
