using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Flags.Models;

namespace Crm.Apps.Flags.Services
{
    public interface IAccountFlagsService
    {
        Task<bool> IsSetAsync(Guid accountId, AccountFlagType type, CancellationToken ct);

        Task<IEnumerable<AccountFlagType>> GetNotSetListAsync(Guid accountId, CancellationToken ct);

        Task SetAsync(Guid accountId, AccountFlagType type, CancellationToken ct);
    }
}
