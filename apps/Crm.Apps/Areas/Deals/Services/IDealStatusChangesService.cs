using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.Parameters;

namespace Crm.Apps.Areas.Deals.Services
{
    public interface IDealStatusChangesService
    {
        Task<List<DealStatusChange>> GetPagedListAsync(DealStatusChangeGetPagedListParameter parameter,
            CancellationToken ct);
    }
}