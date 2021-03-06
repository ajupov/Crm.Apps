using System.Collections.Generic;
using Crm.Apps.Products.Models;

namespace Crm.Apps.Products.V1.Responses
{
    public class ProductCategoryChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<ProductCategoryChange> Changes { get; set; }
    }
}
