using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;

namespace Crm.Clients.Accounts.Clients
{
    public interface IAccountsClient
    {
        Task<Dictionary<AccountType, string>> GetTypesAsync(
            CancellationToken ct = default);

        Task<Account> GetAsync(
            Guid id,
            CancellationToken ct = default);

        Task<Account[]> GetListAsync(
            IEnumerable<Guid> ids,
            CancellationToken ct = default);

        Task<Account[]> GetPagedListAsync(
            bool? isLocked = default,
            bool? isDeleted = default,
            DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default,
            AccountType[] types = default,
            int offset = default,
            int limit = 10,
            string sortBy = "CreateDateTime",
            string orderBy = "desc",
            CancellationToken ct = default);

        Task<Guid> CreateAsync(
            Account account,
            CancellationToken ct = default);

        Task UpdateAsync(
            Account account,
            CancellationToken ct = default);

        Task LockAsync(
            IEnumerable<Guid> ids,
            CancellationToken ct = default);

        Task UnlockAsync(
            IEnumerable<Guid> ids,
            CancellationToken ct = default);

        Task DeleteAsync(
            IEnumerable<Guid> ids,
            CancellationToken ct = default);

        Task RestoreAsync(
            IEnumerable<Guid> ids,
            CancellationToken ct = default);
    }
}