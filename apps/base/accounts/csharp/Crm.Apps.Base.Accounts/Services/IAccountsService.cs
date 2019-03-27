using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Base.Accounts.Models;

namespace Crm.Apps.Base.Accounts.Services
{
    public interface IAccountsService
    {
        Task<Account> GetByIdAsync(Guid id, CancellationToken ct);

        Task<List<Account>> GetListAsync(ICollection<Guid> ids, CancellationToken ct);

        Task<List<Account>> GetPagedListAsync(bool? isLocked, bool? isDeleted, DateTime? minCreateDate,
            DateTime? maxCreateDate, int offset, int limit, string sortBy, string orderBy, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, CancellationToken ct);

        Task UpdateAsync(Guid userId, Account oldAccount, Account newAccount, CancellationToken ct);

        Task LockAsync(Guid userId, ICollection<Guid> ids, CancellationToken ct);

        Task UnlockAsync(Guid userId, ICollection<Guid> ids, CancellationToken ct);

        Task DeleteAsync(Guid userId, ICollection<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, ICollection<Guid> ids, CancellationToken ct);
    }
}