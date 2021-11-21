using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Stock.V1.Requests;
using Crm.Apps.Stock.V1.Responses;

namespace Crm.Apps.Stock.Services
{
    public interface IStockArrivalChangesService
    {
        Task<StockArrivalChangeGetPagedListResponse> GetPagedListAsync(
            StockArrivalChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}
