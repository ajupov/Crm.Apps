﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Crm.Common.UserContext
{
    public class UserContext : IUserContext
    {

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext.User.Claims;

            UserId = Guid.NewGuid();
            AccountId = Guid.NewGuid();
            Name = Guid.NewGuid().ToString();
            AvatarUrl = Guid.NewGuid().ToString();
            Permissions = new List<Permission>();
        }

        public Guid UserId { get; }

        public Guid AccountId { get; }

        public string Name { get; }

        public string AvatarUrl { get; }

        public ICollection<Permission> Permissions { get; }
    }
}