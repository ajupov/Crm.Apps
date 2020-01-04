﻿using System;

namespace Crm.Apps.Users.Models
{
    public class UserGroupLink
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid UserGroupId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}