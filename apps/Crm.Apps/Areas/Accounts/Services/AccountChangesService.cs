using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Areas.Accounts.Models;
using Crm.Apps.Areas.Accounts.RequestParameters;
using Crm.Apps.Areas.Accounts.Storages;
using Crm.Apps.Utils;
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

        public Task<List<AccountChange>> GetPagedListAsync(AccountChangeGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            return _storage.AccountChanges
                .AsNoTracking()
                .Where(x =>
                    (request.ChangerUserId.IsEmpty() || x.ChangerUserId == request.ChangerUserId) &&
                    (request.AccountId.IsEmpty() || x.AccountId == request.AccountId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
        }
    }
}