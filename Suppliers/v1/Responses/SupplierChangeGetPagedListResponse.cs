using System.Collections.Generic;
using Crm.Apps.Suppliers.Models;

namespace Crm.Apps.Suppliers.V1.Responses
{
    public class SupplierChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<SupplierChange> Changes { get; set; }
    }
}
