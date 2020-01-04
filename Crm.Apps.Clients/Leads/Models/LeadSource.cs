using System;

namespace Crm.Clients.Leads.Models
{
    public class LeadSource
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}