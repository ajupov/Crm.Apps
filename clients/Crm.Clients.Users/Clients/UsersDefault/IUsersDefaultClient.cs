using System.Threading;
using System.Threading.Tasks;

namespace Crm.Clients.Users.Clients.UsersDefault
{
    public interface IUsersDefaultClient
    {
        Task StatusAsync(CancellationToken ct = default);
    }
}