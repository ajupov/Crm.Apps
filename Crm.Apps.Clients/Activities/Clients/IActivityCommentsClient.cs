using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.RequestParameters;

namespace Crm.Clients.Activities.Clients
{
    public interface IActivityCommentsClient
    {
        Task<ActivityComment[]> GetPagedListAsync(
            ActivityCommentGetPagedListRequest request,
            CancellationToken ct = default);

        Task CreateAsync(ActivityCommentCreateRequest request, CancellationToken ct = default);
    }
}