using System;
using Crm.Common.Types;

namespace Crm.Clients.Products.Models
{
    public class ProductAttribute
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public AttributeType Type { get; set; }

        public string Key { get; set; }

        public bool IsDeleted { get; set; }
        
        public DateTime CreateDateTime { get; set; }
    }
}