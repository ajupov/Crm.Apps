using System;
using Microsoft.AspNetCore.Http;

namespace Crm.Areas.Accounts.UserContext
{
    public class UserContext : IUserContext
    {
        public UserContext(IHttpContextAccessor accessor)
        {
            var header = accessor.HttpContext.Request.Headers["Authorization"];
            UserId = Guid.NewGuid();
        }

        public Guid UserId { get; }
    }
}