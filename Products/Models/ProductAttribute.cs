using System;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Products.Models
{
    public class ProductAttribute
    {
        [NonDefaultGuid]
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public AttributeType Type { get; set; }

        public string Key { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}
