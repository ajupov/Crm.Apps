﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Activities.Models;
using Crm.Apps.Clients.Activities.RequestParameters;

namespace Crm.Apps.Clients.Activities.Clients
{
    public interface IActivityAttributeChangesClient
    {
        Task<List<ActivityAttributeChange>> GetPagedListAsync(
            ActivityAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}