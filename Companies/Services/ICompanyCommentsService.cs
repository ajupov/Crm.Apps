using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Companies.v1.Models;
using Crm.Apps.Companies.v1.RequestParameters;

namespace Crm.Apps.Companies.Services
{
    public interface ICompanyCommentsService
    {
        Task<List<CompanyComment>> GetPagedListAsync(
            CompanyCommentGetPagedListRequestParameter request,
            CancellationToken ct);

        Task CreateAsync(Guid userId, CompanyComment comment, CancellationToken ct);
    }
}