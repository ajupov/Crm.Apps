using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.v1.Requests;
using Crm.Apps.Companies.v1.Responses;

namespace Crm.Apps.Companies.Services
{
    public interface ICompanyCommentsService
    {
        Task<CompanyCommentGetPagedListResponse> GetPagedListAsync(
            CompanyCommentGetPagedListRequest request,
            CancellationToken ct);

        Task CreateAsync(Guid userId, CompanyComment comment, CancellationToken ct);
    }
}