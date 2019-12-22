using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.RequestParameters;

namespace Crm.Apps.Areas.Deals.Services
{
    public interface IDealChangesService
    {
        Task<List<DealChange>> GetPagedListAsync(DealChangeGetPagedListRequestParameter request, CancellationToken ct);
    }
}