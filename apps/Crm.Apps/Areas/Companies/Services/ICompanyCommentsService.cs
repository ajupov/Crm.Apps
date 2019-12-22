using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Companies.Models;
using Crm.Apps.Areas.Companies.RequestParameters;

namespace Crm.Apps.Areas.Companies.Services
{
    public interface ICompanyCommentsService
    {
        Task<List<CompanyComment>> GetPagedListAsync(
            CompanyCommentGetPagedListRequestParameter request,
            CancellationToken ct);

        Task CreateAsync(Guid userId, CompanyComment comment, CancellationToken ct);
    }
}