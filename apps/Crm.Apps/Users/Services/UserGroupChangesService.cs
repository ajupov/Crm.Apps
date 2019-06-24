using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Users.Helpers;
using Crm.Apps.Users.Models;
using Crm.Apps.Users.Parameters;
using Crm.Apps.Users.Storages;
using Crm.Utils.Guid;
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

        public Task<List<UserGroupChange>> GetPagedListAsync(UserGroupChangeGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.UserGroupChanges.Where(x =>
                    (parameter.ChangerUserId.IsEmpty() || x.ChangerUserId == parameter.ChangerUserId) &&
                    (parameter.GroupId.IsEmpty() || x.GroupId == parameter.GroupId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}