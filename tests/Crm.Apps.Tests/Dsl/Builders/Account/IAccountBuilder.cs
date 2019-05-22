using System.Threading.Tasks;

namespace Crm.Apps.Tests.Dsl.Builders.Account
{
    public interface IAccountBuilder
    {
        AccountBuilder AsLocked();

        AccountBuilder AsDeleted();
        
        AccountBuilder WithSetting(string value);

        Task<Clients.Accounts.Models.Account> BuildAsync();
    }
}