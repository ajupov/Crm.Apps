﻿using System.Linq;
using System.Security.Claims;
using Ajupov.Infrastructure.All.Api;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Auth.Models;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Auth.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireAnyRole(JwtDefaults.AuthenticationScheme)]
    [Route("UserInfo")]
    public class UserInfoController : DefaultApiController
    {
        private readonly IUserContext _userContext;

        public UserInfoController(IUserContext userContext)
        {
            _userContext = userContext;
        }

        [HttpGet("Get")]
        public ActionResult<UserInfo> Get()
        {
            return new UserInfo
            {
                Name = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Name).Value,
                Roles = _userContext.Roles
            };
        }
    }
}