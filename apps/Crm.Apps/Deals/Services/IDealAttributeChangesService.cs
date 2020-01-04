using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.RequestParameters;

namespace Crm.Apps.Deals.Services
{
    public interface IDealAttributeChangesService
    {
        Task<List<DealAttributeChange>> GetPagedListAsync(
            DealAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}