using System.Collections.Generic;
using Crm.Apps.Suppliers.Models;

namespace Crm.Apps.Suppliers.V1.Responses
{
    public class SupplierAttributeChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<SupplierAttributeChange> Changes { get; set; }
    }
}
