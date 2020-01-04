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
    public class UserGroupChangesService : IUserGroupChangesService
    {
        private readonly UsersStorage _storage;

        public UserGroupChangesService(UsersStorage storage)
        {
            _storage = storage;
        }

        public Task<List<UserGroupChange>> GetPagedListAsync(UserGroupChangeGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            return _storage.UserGroupChanges
                .AsNoTracking()
                .Where(x =>
                    (request.ChangerUserId.IsEmpty() || x.ChangerUserId == request.ChangerUserId) &&
                    (request.GroupId.IsEmpty() || x.GroupId == request.GroupId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
        }
    }
}