using System;
using Crm.Common.UserContext;

namespace Crm.Apps.Areas.Users.Models
{
    public class UserPermission
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Permission Permission { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}