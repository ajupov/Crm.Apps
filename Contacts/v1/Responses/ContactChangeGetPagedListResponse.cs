using System.Collections.Generic;
using Crm.Apps.Contacts.Models;

namespace Crm.Apps.Contacts.V1.Responses
{
    public class ContactChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<ContactChange> Changes { get; set; }
    }
}
