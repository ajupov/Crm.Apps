using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Identities.Helpers;
using Crm.Apps.Identities.Models;
using Crm.Apps.Identities.Parameters;
using Crm.Apps.Identities.Storages;
using Crm.Utils.Guid;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Identities.Services
{
    public class IdentityChangesService : IIdentityChangesService
    {
        private readonly IdentitiesStorage _storage;

        public IdentityChangesService(IdentitiesStorage storage)
        {
            _storage = storage;
        }

        public Task<List<IdentityChange>> GetPagedListAsync(IdentityChangeGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.IdentityChanges.Where(x =>
                    (parameter.ChangerUserId.IsEmpty() || x.ChangerUserId == parameter.ChangerUserId) &&
                    (parameter.IdentityId.IsEmpty() || x.IdentityId == parameter.IdentityId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}