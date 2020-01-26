using System;

namespace Crm.Apps.Deals.v1.Models
{
    public class DealPosition
    {
        public Guid Id { get; set; }

        public Guid DealId { get; set; }

        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductVendorCode { get; set; }

        public decimal Price { get; set; }

        public decimal Count { get; set; }
    }
}