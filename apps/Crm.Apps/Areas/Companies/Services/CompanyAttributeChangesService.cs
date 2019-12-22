using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Companies.Helpers;
using Crm.Apps.Areas.Companies.Models;
using Crm.Apps.Areas.Companies.Parameters;
using Crm.Apps.Areas.Companies.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Companies.Services
{
    public class CompanyAttributeChangesService : ICompanyAttributeChangesService
    {
        private readonly CompaniesStorage _storage;

        public CompanyAttributeChangesService(CompaniesStorage storage)
        {
            _storage = storage;
        }

        public Task<List<CompanyAttributeChange>> GetPagedListAsync(
            CompanyAttributeChangeGetPagedListParameter parameter, CancellationToken ct)
        {
            return _storage.CompanyAttributeChanges.Where(x =>
                    (parameter.ChangerUserId.IsEmpty() || x.ChangerUserId == parameter.ChangerUserId) &&
                    (parameter.AttributeId.IsEmpty() || x.AttributeId == parameter.AttributeId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}