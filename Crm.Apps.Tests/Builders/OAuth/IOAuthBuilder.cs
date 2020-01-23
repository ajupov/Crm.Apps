using System.Threading.Tasks;
using Crm.Apps.v1.Clients.OAuth.Models;

namespace Crm.Apps.Tests.Builders.OAuth
{
    public interface IOAuthBuilder
    {
        Task<Tokens> BuildAsync();
    }
}