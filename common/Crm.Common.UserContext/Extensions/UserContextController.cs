using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Common.UserContext.Extensions
{
    public class UserContextController : ControllerBase
    {
        private readonly IUserContext _userContext;

        public UserContextController(IUserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<ActionResult> ActionIfAllowed(
            Func<Task> action,
            Role[] nonPrivilegedRoles,
            params Guid[] accountIds)
        {
            if (_userContext.HasAny(RequirePrivilegedAttribute.PrivilegedRoles))
            {
                await action();

                return NoContent();
            }

            if (_userContext.HasAny(nonPrivilegedRoles) && _userContext.Belongs(accountIds))
            {
                await action();

                return NoContent();
            }

            if (_userContext.HasAny(Role.AccountOwning) && !_userContext.Belongs(accountIds))
            {
                return Forbid();
            }

            throw new Exception();
        }

        public Task<ActionResult> ActionIfAllowed(
            Func<Task> action,
            Role[] nonPrivilegedRoles,
            IEnumerable<Guid> accountIds)
        {
            return ActionIfAllowed(action, nonPrivilegedRoles, accountIds.ToArray());
        }

        public Task<ActionResult> ActionIfAllowed(
            Func<Task> action,
            Role nonPrivilegedRole,
            params Guid[] accountIds)
        {
            return ActionIfAllowed(action, new[] {nonPrivilegedRole}, accountIds);
        }

        public Task<ActionResult> ActionIfAllowed(
            Func<Task> action,
            Role nonPrivilegedRole,
            IEnumerable<Guid> accountIds)
        {
            return ActionIfAllowed(action, new[] {nonPrivilegedRole}, accountIds);
        }

        public ActionResult<TResult> ReturnIfAllowed<TResult>(
            TResult result,
            Role[] nonPrivilegedRoles,
            params Guid[] accountIds)
        {
            if (_userContext.HasAny(RequirePrivilegedAttribute.PrivilegedRoles))
            {
                return result;
            }

            if (_userContext.HasAny(nonPrivilegedRoles) && _userContext.Belongs(accountIds))
            {
                return result;
            }

            if (_userContext.HasAny(nonPrivilegedRoles) && !_userContext.Belongs(accountIds))
            {
                return Forbid();
            }

            throw new Exception();
        }

        public ActionResult<TResult> ReturnIfAllowed<TResult>(
            TResult result,
            Role nonPrivilegedRole,
            IEnumerable<Guid> accountIds)
        {
            return ReturnIfAllowed(result, new[] {nonPrivilegedRole}, accountIds.ToArray());
        }

        public ActionResult<TResult> ReturnIfAllowed<TResult>(
            TResult result,
            Role nonPrivilegedRole,
            params Guid[] accountIds)
        {
            return ReturnIfAllowed(result, new[] {nonPrivilegedRole}, accountIds);
        }

        public ActionResult<TResult> ReturnIfAllowed<TResult>(
            TResult result,
            Role[] nonPrivilegedRoles,
            IEnumerable<Guid> accountIds)
        {
            return ReturnIfAllowed(result, nonPrivilegedRoles, accountIds.ToArray());
        }
    }
}