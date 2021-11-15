using System.Collections.Generic;
using Crm.Apps.Suppliers.Models;

namespace Crm.Apps.Suppliers.V1.Responses
{
    public class SupplierCommentGetPagedListResponse
    {
        public bool HasCommentsBefore { get; set; }

        public List<SupplierComment> Comments { get; set; }
    }
}
