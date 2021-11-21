using System;
using Crm.Common.All.Validation.Attributes;

namespace Crm.Apps.Suppliers.Models
{
    public class SupplierComment
    {
        [NonDefaultGuid]
        public Guid Id { get; set; }

        public Guid SupplierId { get; set; }

        public Guid CommentatorUserId { get; set; }

        public string Value { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
