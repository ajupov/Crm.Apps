﻿using System;
using Crm.Common.UserContext;

namespace Crm.Clients.Users.Models
{
    public class UserGroupPermission
    {
        public Guid Id { get; set; }

        public Guid UserGroupId { get; set; }

        public Role Role { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}