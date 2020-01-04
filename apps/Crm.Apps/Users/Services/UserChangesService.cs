using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Users.Models;
using Crm.Apps.Users.RequestParameters;
using Crm.Apps.Users.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Users.Services
{
    public class UserChangesService : IUserChangesService
    {
        private readonly UsersStorage _storage;

        public UserChangesService(UsersStorage storage)
        {
            _storage = storage;
        }

        public Task<List<UserChange>> GetPagedListAsync(UserChangeGetPagedListRequestParameter request, CancellationToken ct)
        {
            return _storage.UserChanges
                .AsNoTracking()
                .Where(x =>
                    (request.ChangerUserId.IsEmpty() || x.ChangerUserId == request.ChangerUserId) &&
                    (request.UserId.IsEmpty() || x.UserId == request.UserId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
        }
    }
}