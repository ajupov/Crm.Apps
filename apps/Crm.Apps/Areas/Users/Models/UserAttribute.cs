using System;
using System.Collections.Generic;
using Crm.Common.Types;

namespace Crm.Apps.Areas.Users.Models
{
    public class UserAttribute
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public AttributeType Type { get; set; }

        public string Key { get; set; }

        public bool IsDeleted { get; set; }
        
        public DateTime CreateDateTime { get; set; }

        public List<UserAttributeLink> Links { get; set; }

        public List<UserAttributeChange> Changes { get; set; }
    }
}