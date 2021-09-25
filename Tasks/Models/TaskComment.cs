using System;

namespace Crm.Apps.Tasks.Models
{
    public class TaskComment
    {
        public Guid Id { get; set; }

        public Guid TaskId { get; set; }

        public Guid CommentatorUserId { get; set; }

        public string Value { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
