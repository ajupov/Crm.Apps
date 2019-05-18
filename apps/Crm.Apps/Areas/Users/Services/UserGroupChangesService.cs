using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Helpers;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Parameters;
using Crm.Apps.Areas.Users.Storages;
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
            return _storage.UserGroupChanges.Where(x =>
                    (!parameter.ChangerUserId.HasValue || x.ChangerUserId == parameter.ChangerUserId) &&
                    (!parameter.GroupId.HasValue || x.GroupId == parameter.GroupId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}