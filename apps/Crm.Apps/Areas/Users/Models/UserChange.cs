using System;

namespace Crm.Apps.Areas.Users.Models
{
    public class UserChange
    {
        public Guid Id { get; set; }

        public Guid ChangerUserId { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string OldValueJson { get; set; }

        public string NewValueJson { get; set; }
    }
}