using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Accounts.Models;
using Crm.Apps.Accounts.RequestParameters;

namespace Crm.Apps.Accounts.Services
{
    public interface IAccountsService
    {
        Task<Account> GetAsync(Guid id, CancellationToken ct);

        Task<Account[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<Account[]> GetPagedListAsync(AccountGetPagedListRequest request, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, AccountCreateRequest request, CancellationToken ct);

        Task UpdateAsync(Guid userId, Account account, AccountUpdateRequest request, CancellationToken ct);

        Task LockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task UnlockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}