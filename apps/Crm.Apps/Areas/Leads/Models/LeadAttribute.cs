using System;
using Crm.Common.Types;

namespace Crm.Apps.Areas.Leads.Models
{
    public class LeadAttribute
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public AttributeType Type { get; set; }

        public string Key { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}