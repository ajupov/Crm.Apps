using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.RequestParameters;
using Crm.Apps.Areas.Users.Storages;
using Crm.Apps.Utils;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Users.Services
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