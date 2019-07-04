﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.Parameters;

namespace Crm.Apps.Deals.Services
{
    public interface IDealTypeChangesService
    {
        Task<List<DealTypeChange>> GetPagedListAsync(DealTypeChangeGetPagedListParameter parameter,
            CancellationToken ct);
    }
}