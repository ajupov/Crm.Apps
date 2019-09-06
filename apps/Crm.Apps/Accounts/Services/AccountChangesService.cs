using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Accounts.Models;
using Crm.Apps.Accounts.RequestParameters;
using Crm.Apps.Accounts.Storages;
using Crm.Utils.Guid;
using Crm.Utils.Sorting;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Accounts.Services
{
    public class AccountChangesService : IAccountChangesService
    {
        private readonly AccountsStorage _accountsStorage;

        public AccountChangesService(AccountsStorage accountsStorage)
        {
            _accountsStorage = accountsStorage;
        }

        public Task<AccountChange[]> GetPagedListAsync(AccountChangeGetPagedListRequest request, CancellationToken ct)
        {
            return _accountsStorage.AccountChanges
                .Where(x =>
                    (request.ChangerUserId.IsEmpty() || x.ChangerUserId == request.ChangerUserId) &&
                    (request.AccountId.IsEmpty() || x.AccountId == request.AccountId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToArrayAsync(ct);
        }
    }
}