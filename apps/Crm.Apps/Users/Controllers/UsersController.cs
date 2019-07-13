using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Users.Models;
using Crm.Apps.Users.Parameters;
using Crm.Apps.Users.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Utils.Enums;
using Crm.Utils.Guid;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Users.Controllers
{
    [ApiController]
    [Route("Api/Users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IUsersService _usersService;

        public UsersController(IUserContext userContext, IUsersService usersService)
        {
            _userContext = userContext;
            _usersService = usersService;
        }

        [HttpGet("GetGenders")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public ActionResult<List<UserGender>> GetGenders()
        {
            return EnumsExtensions.GetValues<UserGender>().ToList();
        }

        [HttpGet("Get")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult<User>> Get(Guid id, CancellationToken ct = default)
        {
            if (id.IsEmpty())
            {
                return BadRequest();
            }

            var user = await _usersService.GetAsync(id, ct);
            if (user == null)
            {
                return NotFound();
            }

            return ReturnIfAllowed(user, new[] {user.AccountId});
        }

        [HttpPost("GetList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult<List<User>>> GetList(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var users = await _usersService.GetListAsync(ids, ct);

            return ReturnIfAllowed(users, users.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult<List<User>>> GetPagedList(UserGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var users = await _usersService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(users, users.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration)]
        public async Task<ActionResult<Guid>> Create(User user, CancellationToken ct = default)
        {
            if (user == null)
            {
                return BadRequest();
            }

            if (!_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                user.AccountId = _userContext.AccountId;
            }

            var id = await _usersService.CreateAsync(_userContext.UserId, user, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult> Update(User user, CancellationToken ct = default)
        {
            if (user.Id.IsEmpty())
            {
                return BadRequest();
            }

            var oldUser = await _usersService.GetAsync(user.Id, ct);
            if (oldUser == null)
            {
                return NotFound();
            }

            return await ActionIfAllowed(() => _usersService.UpdateAsync(_userContext.UserId, oldUser, user, ct),
                new[] {user.AccountId, oldUser.AccountId});
        }

        [HttpPost("Lock")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult> Lock(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var users = await _usersService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _usersService.LockAsync(_userContext.UserId, users.Select(x => x.Id), ct),
                users.Select(x => x.AccountId));
        }

        [HttpPost("Unlock")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult> Unlock(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var users = await _usersService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _usersService.UnlockAsync(_userContext.UserId, users.Select(x => x.Id), ct),
                users.Select(x => x.AccountId));
        }

        [HttpPost("Delete")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult> Delete(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var users = await _usersService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _usersService.DeleteAsync(_userContext.UserId, users.Select(x => x.Id), ct),
                users.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult> Restore(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var users = await _usersService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _usersService.RestoreAsync(_userContext.UserId, users.Select(x => x.Id), ct),
                users.Select(x => x.AccountId));
        }

        [NonAction]
        private ActionResult<TResult> ReturnIfAllowed<TResult>(TResult result, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                return result;
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning) && _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Permission.AccountOwning) && !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }

        [NonAction]
        private async Task<ActionResult> ActionIfAllowed(Func<Task> action, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                await action();

                return NoContent();
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning) && _userContext.Belongs(accountIdsAsArray))
            {
                await action();

                return NoContent();
            }

            if (_userContext.HasAny(Permission.AccountOwning) && !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}