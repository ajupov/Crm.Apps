using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Contacts.V1.Requests
{
    public class ContactCommentGetPagedListRequest
    {
        [Required]
        public Guid ContactId { get; set; }

        public DateTime? AfterCreateDateTime { get; set; }

        public int Limit { get; set; } = 10;
    }
}
