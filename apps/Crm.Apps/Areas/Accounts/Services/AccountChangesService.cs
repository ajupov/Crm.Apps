using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Accounts.Helpers;
using Crm.Apps.Areas.Accounts.Models;
using Crm.Apps.Areas.Accounts.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Accounts.Services
{
    public class AccountChangesService : IAccountChangesService
    {
        private readonly AccountsStorage _storage;

        public AccountChangesService(AccountsStorage storage)
        {
            _storage = storage;
        }

        public Task<List<AccountChange>> GetPagedListAsync(Guid? changerUserId, Guid? accountId,
            DateTime? minCreateDate, DateTime? maxCreateDate, int offset, int limit, string sortBy, string orderBy,
            CancellationToken ct)
        {
            return _storage.AccountChanges.Where(x =>
                    (!changerUserId.HasValue || x.ChangerUserId == changerUserId) &&
                    (!accountId.HasValue || x.AccountId == accountId) &&
                    (!minCreateDate.HasValue || x.CreateDateTime >= minCreateDate) &&
                    (!maxCreateDate.HasValue || x.CreateDateTime <= maxCreateDate))
                .Sort(sortBy, orderBy)
                .Skip(offset)
                .Take(limit)
                .ToListAsync(ct);
        }
    }
}