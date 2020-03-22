using System.Collections.Generic;
using Crm.Apps.Contacts.Models;

namespace Crm.Apps.Contacts.v1.Responses
{
    public class ContactCommentGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<ContactComment> Comments { get; set; }
    }
}