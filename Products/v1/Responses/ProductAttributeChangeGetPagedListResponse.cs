using System.Collections.Generic;
using Crm.Apps.Products.Models;

namespace Crm.Apps.Products.V1.Responses
{
    public class ProductAttributeChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<ProductAttributeChange> Changes { get; set; }
    }
}
