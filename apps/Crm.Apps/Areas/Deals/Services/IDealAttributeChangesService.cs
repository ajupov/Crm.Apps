using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.Parameters;

namespace Crm.Apps.Areas.Deals.Services
{
    public interface IDealAttributeChangesService
    {
        Task<List<DealAttributeChange>> GetPagedListAsync(DealAttributeChangeGetPagedListParameter parameter,
            CancellationToken ct);
    }
}