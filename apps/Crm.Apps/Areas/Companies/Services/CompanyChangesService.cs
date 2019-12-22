using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Areas.Companies.Models;
using Crm.Apps.Areas.Companies.RequestParameters;
using Crm.Apps.Areas.Companies.Storages;
using Crm.Apps.Utils;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Companies.Services
{
    public class CompanyChangesService : ICompanyChangesService
    {
        private readonly CompaniesStorage _storage;

        public CompanyChangesService(CompaniesStorage storage)
        {
            _storage = storage;
        }

        public Task<List<CompanyChange>> GetPagedListAsync(
            CompanyChangeGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            return _storage.CompanyChanges
                .AsNoTracking()
                .Where(x =>
                    (request.ChangerUserId.IsEmpty() || x.ChangerUserId == request.ChangerUserId) &&
                    (request.CompanyId.IsEmpty() || x.CompanyId == request.CompanyId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
        }
    }
}