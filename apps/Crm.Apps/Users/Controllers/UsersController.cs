using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.ApiDocumentation.Attributes;
using Ajupov.Utils.All.Enums;
using Crm.Apps.Users.Models;
using Crm.Apps.Users.RequestParameters;
using Crm.Apps.Users.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Users.Controllers
{
    [ApiController]
    [RequirePrivileged]
    [IgnoreApiDocumentation]
    [Route("Api/Users")]
    public class UsersController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IUsersService _usersService;

        public UsersController(IUserContext userContext, IUsersService usersService)
            : base(userContext)
        {
            _userContext = userContext;
            _usersService = usersService;
        }

        [HttpGet("GetGenders")]
        public ActionResult<Dictionary<string, UserGender>> GetGenders()
        {
            return EnumsExtensions.GetAsDictionary<UserGender>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<User>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var user = await _usersService.GetAsync(id, ct);
            if (user == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(user, Role.AccountOwning, user.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<User>>> GetList([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var users = await _usersService.GetListAsync(ids, ct);

            return ReturnIfAllowed(users, Role.AccountOwning, users.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<User>>> GetPagedList(
            UserGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var users = await _usersService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(users, Role.AccountOwning, users.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(User user, CancellationToken ct = default)
        {
            user.AccountId = _userContext.AccountId;

            var id = await _usersService.CreateAsync(_userContext.UserId, user, ct);

            return Created("Get", id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(User user, CancellationToken ct = default)
        {
            var oldUser = await _usersService.GetAsync(user.Id, ct);
            if (oldUser == null)
            {
                return NotFound(user.Id);
            }

            return await ActionIfAllowed(
                () => _usersService.UpdateAsync(_userContext.UserId, oldUser, user, ct),
                Role.AccountOwning,
                user.AccountId, oldUser.AccountId);
        }

        [HttpPost("Lock")]
        public async Task<ActionResult> Lock([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var users = await _usersService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _usersService.LockAsync(_userContext.UserId, users.Select(x => x.Id), ct),
                Role.AccountOwning,
                users.Select(x => x.AccountId));
        }

        [HttpPost("Unlock")]
        public async Task<ActionResult> Unlock([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var users = await _usersService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _usersService.UnlockAsync(_userContext.UserId, users.Select(x => x.Id), ct),
                Role.AccountOwning,
                users.Select(x => x.AccountId));
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var users = await _usersService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _usersService.DeleteAsync(_userContext.UserId, users.Select(x => x.Id), ct),
                Role.AccountOwning,
                users.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var users = await _usersService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _usersService.RestoreAsync(_userContext.UserId, users.Select(x => x.Id), ct),
                Role.AccountOwning,
                users.Select(x => x.AccountId));
        }
    }
}