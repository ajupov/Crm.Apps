using System;

namespace Crm.Apps.v1.Clients.Products.Models
{
    public class ProductAttributeLink
    {
        // public Guid Id { get; set; }
        //
        // public Guid ProductId { get; set; }

        public Guid ProductAttributeId { get; set; }

        public string Value { get; set; }
    }
}