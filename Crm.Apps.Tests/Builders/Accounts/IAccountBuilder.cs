using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;

namespace Crm.Apps.Tests.Builders.Accounts
{
    public interface IAccountBuilder
    {
        AccountBuilder WithType(AccountType type);

        AccountBuilder AsLocked();

        AccountBuilder AsDeleted();

        AccountBuilder WithSetting(AccountSettingType type, string? value = null);

        Task<Account> BuildAsync();
    }
}