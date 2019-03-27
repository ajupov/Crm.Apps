using System.Reflection;
using System.Threading.Tasks;
using Crm.Apps.Base.Accounts.Consumers;
using Crm.Apps.Base.Accounts.Services;
using Crm.Apps.Base.Accounts.Storages;
using Crm.Common.UserContext;
using Crm.Infrastructure.All;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Apps.Base.Accounts
{
    public static class Program
    {
        public static Task Main()
        {
            return Assembly
                .GetExecutingAssembly()
                .CreateAndRunAsync<AccountsStorage, AccountsConsumer, IUserContext, UserContext>(
                    services => services.AddSingleton<IAccountsService, AccountsService>(), "Accounts");
        }
    }
}