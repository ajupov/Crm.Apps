using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Parameters;
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

        public Task<List<UserGroupChange>> GetPagedListAsync(UserGroupChangeGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.UserGroupChanges
                .AsNoTracking()
                .Where(x =>
                    (parameter.ChangerUserId.IsEmpty() || x.ChangerUserId == parameter.ChangerUserId) &&
                    (parameter.GroupId.IsEmpty() || x.GroupId == parameter.GroupId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .SortBy(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}