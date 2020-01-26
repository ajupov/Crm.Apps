using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.Guid;
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
            var isNew = link.Id.IsEmpty();

            return new ContactAttributeLink
            {
                Id = isNew ? Guid.NewGuid() : link.Id,
                ContactId = contactId,
                ContactAttributeId = link.ContactAttributeId,
                Value = link.Value,
                CreateDateTime = isNew ? DateTime.UtcNow : link.CreateDateTime,
                ModifyDateTime = isNew ? (DateTime?) null : DateTime.UtcNow
            };
        }
    }
}