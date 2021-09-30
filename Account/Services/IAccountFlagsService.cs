using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Account.Models;

namespace Crm.Apps.Account.Services
{
    public interface IAccountFlagsService
    {
        Task<bool> IsSetAsync(Guid accountId, AccountFlagType type, CancellationToken ct);

        Task<List<AccountFlagType>> GetNotSetListAsync(Guid accountId, CancellationToken ct);

        Task SetAsync(Guid accountId, AccountFlagType type, CancellationToken ct);
    }
}
