using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Accounts.Models;
using Crm.Apps.Areas.Accounts.RequestParameters;

namespace Crm.Apps.Areas.Accounts.Services
{
    public interface IAccountsService
    {
        Task<Account> GetAsync(Guid id, CancellationToken ct);

        Task<List<Account>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<Account>> GetPagedListAsync(AccountGetPagedListRequestParameter request, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, Account account, CancellationToken ct);

        Task UpdateAsync(Guid userId, Account oldAccount, Account newAccount, CancellationToken ct);

        Task LockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task UnlockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}