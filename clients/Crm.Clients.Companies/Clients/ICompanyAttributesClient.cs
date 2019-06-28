using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Companies.Models;
using Crm.Common.Types;

namespace Crm.Clients.Companies.Clients
{
    public interface ICompanyAttributesClient
    {
        Task<List<AttributeType>> GetTypesAsync(CancellationToken ct = default);

        Task<CompanyAttribute> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<CompanyAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<CompanyAttribute>> GetPagedListAsync(Guid? accountId = default, List<AttributeType> types = default,
            string key = default, bool? isDeleted = default, DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default, int offset = default, int limit = 10, string sortBy = default,
            string orderBy = default, CancellationToken ct = default);

        Task<Guid> CreateAsync(CompanyAttribute attribute, CancellationToken ct = default);

        Task UpdateAsync(CompanyAttribute attribute, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}