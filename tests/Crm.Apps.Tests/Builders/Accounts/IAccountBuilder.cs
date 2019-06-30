using System.Threading.Tasks;

namespace Crm.Apps.Tests.Builders.Accounts
{
    public interface IAccountBuilder
    {
        AccountBuilder AsLocked();

        AccountBuilder AsDeleted();
        
        AccountBuilder WithSetting(string value);

        Task<Clients.Accounts.Models.Account> BuildAsync();
    }
}