using System.Collections.Generic;
using Crm.Apps.Products.Models;

namespace Crm.Apps.Products.v1.Responses
{
    public class ProductStatusChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<ProductStatusChange> Changes { get; set; }
    }
}