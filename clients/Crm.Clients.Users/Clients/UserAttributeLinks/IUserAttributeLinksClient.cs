using System;
using System.Threading;
using System.Threading.Tasks;

namespace Crm.Clients.Users.Clients.UserAttributeLinks
{
    public interface IUserAttributeLinksClient
    {
        Task SetAsync(Guid userId, Guid attributeId, string value = null, CancellationToken ct = default);

        Task ResetAsync(Guid userId, Guid attributeId, CancellationToken ct = default);
    }
}