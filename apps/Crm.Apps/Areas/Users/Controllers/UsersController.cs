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
        public async Task<ActionResult<User>> Get(
            [FromQuery] Guid id,
            CancellationToken ct = default)
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

            return ReturnIfAllowed(user, user.AccountId);
        }

        [HttpGet("GetList")]
        public async Task<ActionResult<ICollection<User>>> GetList(
            [FromQuery] ICollection<Guid> ids,
            CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            var users = await _usersService.GetListAsync(ids, ct).ConfigureAwait(false);

            return ReturnIfAllowed(users, users.Select(x => x.AccountId).ToArray());
        }

        [HttpGet("GetPagedList")]
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

            if (_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                return users;
            }

            if (_userContext.HasAny(Permission.AccountOwning))
            {
                if (_userContext.Belongs(users.Select(x => x.AccountId).ToArray()))
                {
                    return users;
                }

                return Forbid();
            }

            throw new Exception();
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(User user, CancellationToken ct = default)
        {
            var id = await _usersService.CreateAsync(_userContext.UserId, _userContext.AccountId, user, ct)
                .ConfigureAwait(false);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult<Guid>> Update(
            [FromBody] User account,
            CancellationToken ct = default)
        {
            if (account.Id == Guid.Empty)
            {
                return BadRequest();
            }

            var oldUser = await _usersService.GetAsync(account.Id, ct).ConfigureAwait(false);
            if (oldUser == null)
            {
                return NotFound();
            }

            await _usersService.UpdateAsync(_userContext.UserId, oldUser, account, ct).ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("Lock")]
        public async Task<ActionResult> Lock(
            [FromBody] ICollection<Guid> ids,
            CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            await _usersService.LockAsync(_userContext.UserId, ids, ct).ConfigureAwait(false);

            return NoContent();
        }

        [
            HttpPost("Unlock")]
        public async Task<ActionResult> Unlock(
            [FromBody] ICollection<Guid> ids,
            CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            await _usersService.UnlockAsync(_userContext.UserId, ids, ct).ConfigureAwait(false);

            return NoContent();
        }

        [
            HttpPost("Delete")]
        public async Task<ActionResult> Delete(
            [FromBody] ICollection<Guid> ids,
            CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            await _usersService.DeleteAsync(_userContext.UserId, ids, ct).ConfigureAwait(false);

            return NoContent();
        }

        [
            HttpPost("Restore")]
        public async Task<ActionResult> Restore(
            [FromBody] ICollection<Guid> ids,
            CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            await _usersService.RestoreAsync(_userContext.UserId, ids, ct).ConfigureAwait(false);

            return NoContent();
        }

        private ActionResult<T> ReturnIfAllowed<T>(T result, params Guid[] accountIds)
        {
            if (_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                return result;
            }

            if (_userContext.HasAny(Permission.AccountOwning) && _userContext.Belongs(accountIds))
            {
                return result;
            }

            if (_userContext.HasAny(Permission.AccountOwning) && !_userContext.Belongs(accountIds))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}