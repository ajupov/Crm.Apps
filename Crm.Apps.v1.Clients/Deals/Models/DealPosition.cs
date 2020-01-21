using System;

namespace Crm.Apps.v1.Clients.Deals.Models
{
    public class DealPosition
    {
        public Guid Id { get; set; }

        public Guid DealId { get; set; }

        public Guid ProductId { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }
    }
}