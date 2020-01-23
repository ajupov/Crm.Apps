using System.Threading.Tasks;

namespace Crm.Apps.Tests.Services.AccessTokenGetter
{
    public interface IAccessTokenGetter
    {
        Task<string> GetAsync();
    }
}