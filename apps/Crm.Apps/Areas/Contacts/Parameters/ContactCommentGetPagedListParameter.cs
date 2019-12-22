using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Areas.Contacts.Parameters
{
    public class ContactCommentGetPagedListParameter
    {
        [Required]
        public Guid ContactId { get; set; }

        public Guid? CommentatorUserId { get; set; }

        public string Value { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}