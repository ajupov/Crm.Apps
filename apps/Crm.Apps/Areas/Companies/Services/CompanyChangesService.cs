using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Areas.Companies.Models;
using Crm.Apps.Areas.Companies.Parameters;
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
            CompanyChangeGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.CompanyChanges
                .AsNoTracking()
                .Where(x =>
                    (parameter.ChangerUserId.IsEmpty() || x.ChangerUserId == parameter.ChangerUserId) &&
                    (parameter.CompanyId.IsEmpty() || x.CompanyId == parameter.CompanyId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .SortBy(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}