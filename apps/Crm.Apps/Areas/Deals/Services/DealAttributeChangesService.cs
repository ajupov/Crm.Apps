using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Helpers;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.Parameters;
using Crm.Apps.Areas.Deals.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Deals.Services
{
    public class DealAttributeChangesService : IDealAttributeChangesService
    {
        private readonly DealsStorage _storage;

        public DealAttributeChangesService(DealsStorage storage)
        {
            _storage = storage;
        }

        public Task<List<DealAttributeChange>> GetPagedListAsync(
            DealAttributeChangeGetPagedListParameter parameter, CancellationToken ct)
        {
            return _storage.DealAttributeChanges.Where(x =>
                    (parameter.ChangerUserId.IsEmpty() || x.ChangerUserId == parameter.ChangerUserId) &&
                    (parameter.AttributeId.IsEmpty() || x.AttributeId == parameter.AttributeId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}