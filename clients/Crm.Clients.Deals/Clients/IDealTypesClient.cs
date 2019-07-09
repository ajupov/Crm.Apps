﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Deals.Models;

namespace Crm.Clients.Deals.Clients
{
    public interface IDealTypesClient
    {
        Task<DealType> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<DealType>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<DealType>> GetPagedListAsync(Guid? accountId = default, string name = default,
            bool? isDeleted = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(DealType type, CancellationToken ct = default);

        Task UpdateAsync(DealType type, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}