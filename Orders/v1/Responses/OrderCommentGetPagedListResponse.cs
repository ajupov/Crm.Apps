using System.Collections.Generic;
using Crm.Apps.Orders.Models;

namespace Crm.Apps.Orders.V1.Responses
{
    public class OrderCommentGetPagedListResponse
    {
        public bool HasCommentsBefore { get; set; }

        public List<OrderComment> Comments { get; set; }
    }
}
