using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.Parameters;

namespace Crm.Apps.Companies.Services
{
    public interface ICompanyCommentsService
    {
        Task<List<CompanyComment>> GetPagedListAsync(CompanyCommentGetPagedListParameter parameter,
            CancellationToken ct);

        Task CreateAsync(Guid userId, CompanyComment comment, CancellationToken ct);
    }
}