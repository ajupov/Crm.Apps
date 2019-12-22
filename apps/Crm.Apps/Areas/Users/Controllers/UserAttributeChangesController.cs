using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Parameters;
using Crm.Apps.Areas.Users.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.Extensions;
using Crm.Infrastructure.ApiDocumentation.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Users.Controllers
{
    [ApiController]
    [RequirePrivileged]
    [IgnoreApiDocumentation]
    [Route("Api/Users/Attributes/Changes")]
    public class UserAttributeChangesController : UserContextController
    {
        private readonly IUserAttributesService _userAttributesService;
        private readonly IUserAttributeChangesService _userAttributeChangesService;

        public UserAttributeChangesController(
            IUserContext userContext,
            IUserAttributeChangesService userAttributeChangesService,
            IUserAttributesService userAttributesService)
            : base(userContext)
        {
            _userAttributeChangesService = userAttributeChangesService;
            _userAttributesService = userAttributesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<UserAttributeChange>>> GetPagedList(
            UserAttributeChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var attribute = await _userAttributesService.GetAsync(parameter.AttributeId, ct);
            var changes = await _userAttributeChangesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(changes, Role.AccountOwning, attribute.AccountId);
        }
    }
}