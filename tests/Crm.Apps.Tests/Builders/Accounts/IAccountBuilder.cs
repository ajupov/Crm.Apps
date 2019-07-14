using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;

namespace Crm.Apps.Tests.Builders.Accounts
{
    public interface IAccountBuilder
    {
        AccountBuilder AsLocked();

        AccountBuilder AsDeleted();

        AccountBuilder WithSetting(
            AccountSettingType type,
            string value);

        Task<Account> BuildAsync();
    }
}