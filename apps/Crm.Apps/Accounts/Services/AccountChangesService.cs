using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Accounts.Models;
using Crm.Apps.Accounts.Parameters;
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

        public Task<AccountChange[]> GetPagedListAsync(
            AccountChangeGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _accountsStorage.AccountChanges.Where(x =>
                    (parameter.ChangerUserId.IsEmpty() || x.ChangerUserId == parameter.ChangerUserId) &&
                    (parameter.AccountId.IsEmpty() || x.AccountId == parameter.AccountId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToArrayAsync(ct);
        }
    }
}