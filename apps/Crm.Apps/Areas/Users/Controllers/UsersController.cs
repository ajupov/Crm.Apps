using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Users.Controllers
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

        [HttpGet("Get")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult<User>> Get([FromQuery] Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var user = await _usersService.GetAsync(id, ct).ConfigureAwait(false);
            if (user == null)
            {
                return NotFound();
            }

            return ReturnIfAllowed(user, new[] {user.AccountId});
        }

        [HttpGet("GetList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult<ICollection<User>>> GetList([FromQuery] ICollection<Guid> ids,
            CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            var users = await _usersService.GetListAsync(ids, ct).ConfigureAwait(false);

            return ReturnIfAllowed<ICollection<User>>(users, users.Select(x => x.AccountId));
        }

        [HttpGet("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult<ICollection<User>>> GetPagedList(
            [FromQuery] Guid? accountId = default,
            [FromQuery] string surname = default,
            [FromQuery] string name = default,
            [FromQuery] string patronymic = default,
            [FromQuery] DateTime? minBirthDate = default,
            [FromQuery] DateTime? maxBirthDate = default,
            [FromQuery] UserGender? gender = default,
            [FromQuery] bool? isLocked = default,
            [FromQuery] bool? isDeleted = default,
            [FromQuery] DateTime? minCreateDate = default,
            [FromQuery] DateTime? maxCreateDate = default,
            [FromQuery] bool? allAttributes = default,
            [FromQuery] IDictionary<Guid, string> attributes = default,
            [FromQuery] bool? allPermissions = default,
            [FromQuery] ICollection<Permission> permissions = default,
            [FromQuery] bool? allGroupIds = default,
            [FromQuery] ICollection<Guid> groupIds = default,
            [FromQuery] int offset = default,
            [FromQuery] int limit = 10,
            [FromQuery] string sortBy = default,
            [FromQuery] string orderBy = default,
            CancellationToken ct = default)
        {
            var users = await _usersService.GetPagedListAsync(accountId, surname, name, patronymic, minBirthDate,
                    maxBirthDate, gender, isLocked, isDeleted, minCreateDate, maxCreateDate, allAttributes, attributes,
                    allPermissions, permissions, allGroupIds, groupIds, offset, limit, sortBy, orderBy, ct)
                .ConfigureAwait(false);

            return ReturnIfAllowed<ICollection<User>>(users, users.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration)]
        public async Task<ActionResult<Guid>> Create([FromBody] User user, CancellationToken ct = default)
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

            var id = await _usersService.CreateAsync(_userContext.UserId, user, ct).ConfigureAwait(false);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult> Update([FromBody] User user, CancellationToken ct = default)
        {
            if (user.Id == Guid.Empty)
            {
                return BadRequest();
            }

            var oldUser = await _usersService.GetAsync(user.Id, ct).ConfigureAwait(false);
            if (oldUser == null)
            {
                return NotFound();
            }

            return await ActionIfAllowed(() => _usersService.UpdateAsync(_userContext.UserId, oldUser, user, ct),
                new[] {oldUser.AccountId});
        }

        [HttpPost("Lock")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult> Lock([FromBody] ICollection<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            var users = await _usersService.GetListAsync(ids, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _usersService.LockAsync(_userContext.UserId, users.Select(x => x.Id), ct),
                users.Select(x => x.AccountId));
        }

        [HttpPost("Unlock")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult> Unlock([FromBody] ICollection<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            var users = await _usersService.GetListAsync(ids, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _usersService.UnlockAsync(_userContext.UserId, users.Select(x => x.Id), ct),
                users.Select(x => x.AccountId));
        }

        [HttpPost("Delete")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult> Delete([FromBody] ICollection<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            var users = await _usersService.GetListAsync(ids, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _usersService.DeleteAsync(_userContext.UserId, users.Select(x => x.Id), ct),
                users.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult> Restore([FromBody] ICollection<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            var users = await _usersService.GetListAsync(ids, ct).ConfigureAwait(false);

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
                await action().ConfigureAwait(false);

                return NoContent();
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning) && _userContext.Belongs(accountIdsAsArray))
            {
                await action().ConfigureAwait(false);

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