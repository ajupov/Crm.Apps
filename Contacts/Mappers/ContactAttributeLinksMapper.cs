using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Contacts.v1.Models;

namespace Crm.Apps.Contacts.Mappers
{
    public static class ContactAttributeLinksMapper
    {
        public static List<ContactAttributeLink> Map(this List<ContactAttributeLink> links, Guid contactId)
        {
            return links?
                .Select(l => Map(l, contactId))
                .ToList();
        }

        public static ContactAttributeLink Map(this ContactAttributeLink link, Guid contactId)
        {
            return new ContactAttributeLink
            {
                Id = link.Id,
                ContactId = contactId,
                ContactAttributeId = link.ContactAttributeId,
                Value = link.Value
            };
        }
    }
}