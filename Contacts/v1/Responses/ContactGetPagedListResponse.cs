using System;
using System.Collections.Generic;
using Crm.Apps.Contacts.Models;

namespace Crm.Apps.Contacts.V1.Responses
{
    public class ContactGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public DateTime? LastModifyDateTime { get; set; }

        public List<Contact> Contacts { get; set; }
    }
}
