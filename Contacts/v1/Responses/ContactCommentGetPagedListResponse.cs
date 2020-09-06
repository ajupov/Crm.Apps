using System.Collections.Generic;
using Crm.Apps.Contacts.Models;

namespace Crm.Apps.Contacts.V1.Responses
{
    public class ContactCommentGetPagedListResponse
    {
        public List<ContactComment> Comments { get; set; }
    }
}
