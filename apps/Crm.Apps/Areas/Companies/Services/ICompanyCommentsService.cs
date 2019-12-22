using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Companies.Models;
using Crm.Apps.Areas.Companies.Parameters;

namespace Crm.Apps.Areas.Companies.Services
{
    public interface ICompanyCommentsService
    {
        Task<List<CompanyComment>> GetPagedListAsync(CompanyCommentGetPagedListParameter parameter,
            CancellationToken ct);

        Task CreateAsync(Guid userId, CompanyComment comment, CancellationToken ct);
    }
}