using System;

namespace Crm.Clients.Companies.Models
{
    public class CompanyAttributeLink
    {
        public Guid Id { get; set; }

        public Guid CompanyId { get; set; }

        public Guid CompanyAttributeId { get; set; }

        public string Value { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}