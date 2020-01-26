using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Deals.Storages;
using Crm.Apps.Deals.v1.Models;
using Crm.Apps.Deals.v1.RequestParameters;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Deals.Services
{
    public class DealAttributeChangesService : IDealAttributeChangesService
    {
        private readonly DealsStorage _storage;

        public DealAttributeChangesService(DealsStorage storage)
        {
            _storage = storage;
        }

        public Task<List<DealAttributeChange>> GetPagedListAsync(
            DealAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            return _storage.DealAttributeChanges
                .Where(x =>
                    (request.ChangerUserId.IsEmpty() || x.ChangerUserId == request.ChangerUserId) &&
                    (request.AttributeId.IsEmpty() || x.AttributeId == request.AttributeId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
        }
    }
}