using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;
using Crm.Clients.Accounts.RequestParameters;

namespace Crm.Clients.Accounts.Clients
{
    public interface IAccountsClient
    {
        Task<Dictionary<string, AccountType>> GetTypesAsync(CancellationToken ct = default);

        Task<Account> GetAsync(Guid id, CancellationToken ct = default);

        Task<Account[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<Account[]> GetPagedListAsync(AccountGetPagedListParameter request, CancellationToken ct = default);

        Task<Guid> CreateAsync(AccountCreateRequest request, CancellationToken ct = default);

        Task UpdateAsync(AccountUpdateRequest request, CancellationToken ct = default);

        Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}