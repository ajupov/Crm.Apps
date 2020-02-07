using System.Linq;
using System.Security.Claims;
using Ajupov.Infrastructure.All.ApiDocumentation.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc;
using Crm.Apps.Auth.Models;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Auth.Controllers
{
    [ApiController]
    [IgnoreApiDocumentation]
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