using System.Collections.Generic;
using Crm.Apps.Products.v1.Models;

namespace Crm.Apps.Products.v1.Responses
{
    public class ProductChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<ProductChange> Changes { get; set; }
    }
}