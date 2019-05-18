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
    public class UserChangesService : IUserChangesService
    {
        private readonly UsersStorage _storage;

        public UserChangesService(UsersStorage storage)
        {
            _storage = storage;
        }

        public Task<List<UserChange>> GetPagedListAsync(UserChangeGetPagedListParameter parameter, CancellationToken ct)
        {
            return _storage.UserChanges.Where(x =>
                    (!parameter.ChangerUserId.HasValue || x.ChangerUserId == parameter.ChangerUserId) &&
                    (!parameter.UserId.HasValue || x.UserId == parameter.UserId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}