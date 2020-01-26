﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Activities.Models;
using Crm.Apps.v1.Clients.Activities.RequestParameters;

namespace Crm.Apps.v1.Clients.Activities.Clients
{
    public interface IActivityStatusesClient
    {
        Task<ActivityStatus> GetAsync(string accessToken, Guid id, CancellationToken ct = default);

        Task<List<ActivityStatus>> GetListAsync(
            string accessToken,
            IEnumerable<Guid> ids,
            CancellationToken ct = default);

        Task<List<ActivityStatus>> GetPagedListAsync(
            string accessToken,
            ActivityStatusGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(string accessToken, ActivityStatus status, CancellationToken ct = default);

        Task UpdateAsync(string accessToken, ActivityStatus status, CancellationToken ct = default);

        Task DeleteAsync(string accessToken, IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(string accessToken, IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}