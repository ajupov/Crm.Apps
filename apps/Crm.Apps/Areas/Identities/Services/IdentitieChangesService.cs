using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Accounts.Helpers;
using Crm.Apps.Areas.Accounts.Models;
using Crm.Apps.Areas.Accounts.Parameters;
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

        public Task<List<AccountChange>> GetPagedListAsync(AccountChangeGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.AccountChanges.Where(x =>
                    (!parameter.ChangerUserId.HasValue || x.ChangerUserId == parameter.ChangerUserId) &&
                    (!parameter.AccountId.HasValue || x.AccountId == parameter.AccountId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}