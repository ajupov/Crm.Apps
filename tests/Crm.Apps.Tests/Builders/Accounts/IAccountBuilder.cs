using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;

namespace Crm.Apps.Tests.Builders.Accounts
{
    public interface IAccountBuilder
    {
        AccountBuilder AsLocked();

        AccountBuilder AsDeleted();
        
        AccountBuilder WithSetting(string value);

        Task<Account> BuildAsync();
    }
}