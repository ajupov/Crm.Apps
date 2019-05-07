using System;

namespace Crm.Apps.Areas.Users.Models
{
    public class UserGroupLink
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid GroupId { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}