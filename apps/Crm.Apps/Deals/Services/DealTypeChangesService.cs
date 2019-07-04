using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.Helpers;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.Parameters;
using Crm.Apps.Deals.Storages;
using Crm.Utils.Guid;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Deals.Services
{
    public class DealTypeChangesService : IDealTypeChangesService
    {
        private readonly DealsStorage _storage;

        public DealTypeChangesService(DealsStorage storage)
        {
            _storage = storage;
        }

        public Task<List<DealTypeChange>> GetPagedListAsync(DealTypeChangeGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.DealTypeChanges.Where(x =>
                    (parameter.ChangerUserId.IsEmpty() || x.ChangerUserId == parameter.ChangerUserId) &&
                    (parameter.TypeId.IsEmpty() || x.TypeId == parameter.TypeId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}