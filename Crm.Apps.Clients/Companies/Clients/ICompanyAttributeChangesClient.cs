using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Companies.Models;

namespace Crm.Apps.Clients.Companies.Clients
{
    public interface ICompanyAttributeChangesClient
    {
        Task<List<CompanyAttributeChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? attributeId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default, int limit = 10,
            string sortBy = default, string orderBy = default, CancellationToken ct = default);
    }
}