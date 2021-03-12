using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.User.Storages;
using Crm.Apps.User.V1.Requests;
using Crm.Apps.User.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.User.Services
{
    public class UserSettingChangesService : IUserSettingChangesService
    {
        private readonly UserStorage _storage;

        public UserSettingChangesService(UserStorage storage)
        {
            _storage = storage;
        }

        public async Task<UserSettingChangeGetPagedListResponse> GetPagedListAsync(
            Guid userId,
            UserSettingChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.UserSettingChanges
                .AsNoTracking()
                .Where(x =>
                    x.UserId == userId &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new UserSettingChangeGetPagedListResponse
            {
                TotalCount = await changes
                    .CountAsync(ct),
                Changes = await changes
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
        }
    }
}
