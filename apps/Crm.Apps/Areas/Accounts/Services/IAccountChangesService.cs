using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Accounts.Models;

namespace Crm.Apps.Areas.Accounts.Services
{
    public interface IAccountChangesService
    {
        Task<List<AccountChange>> GetPagedListAsync(Guid? changerUserId, Guid? accountId, DateTime? minCreateDate,
            DateTime? maxCreateDate, int offset, int limit, string sortBy, string orderBy, CancellationToken ct);
    }
}