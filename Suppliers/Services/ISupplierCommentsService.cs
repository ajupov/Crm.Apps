using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Suppliers.Models;
using Crm.Apps.Suppliers.V1.Requests;
using Crm.Apps.Suppliers.V1.Responses;

namespace Crm.Apps.Suppliers.Services
{
    public interface ISupplierCommentsService
    {
        Task<SupplierCommentGetPagedListResponse> GetPagedListAsync(
            SupplierCommentGetPagedListRequest request,
            CancellationToken ct);

        Task CreateAsync(Guid userId, SupplierComment comment, CancellationToken ct);
    }
}
