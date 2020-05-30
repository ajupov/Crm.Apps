using System.Collections.Generic;
using Crm.Apps.Contacts.Models;

namespace Crm.Apps.Contacts.V1.Responses
{
    public class ContactAttributeChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<ContactAttributeChange> Changes { get; set; }
    }
}
