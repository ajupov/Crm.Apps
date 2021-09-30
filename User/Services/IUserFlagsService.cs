using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.User.Models;

namespace Crm.Apps.User.Services
{
    public interface IUserFlagsService
    {
        Task<bool> IsSetAsync(Guid userId, UserFlagType type, CancellationToken ct);

        Task<List<UserFlagType>> GetNotSetListAsync(Guid userId, CancellationToken ct);

        Task SetAsync(Guid userId, UserFlagType type, CancellationToken ct);
    }
}
