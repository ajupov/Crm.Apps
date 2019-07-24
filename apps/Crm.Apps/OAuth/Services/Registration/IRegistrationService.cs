using System;
using System.Threading.Tasks;
using Crm.Modules.Accounts.Models;
using Crm.Modules.Identities.Enums;
using Crm.Modules.Identities.Models;
using Crm.Modules.Users.Enums;
using Crm.Modules.Users.Models;

namespace Crm.Businness.Registration
{
    public interface IRegistrationService
    {
        Task<Account> CreateAccountAsync();

        Task<User> CreateUserAsync(Account account, string surname = null, string name = null,
            DateTime? birthDate = null, UserGender gender = UserGender.None);

        Task<Identity> CreateLoginIdentityAsync(User user, string login, string passwordHash);

        Task<Identity> CreateEmailIdentityAsync(User user, string email, string passwordHash, bool needVerify = true);

        Task<Identity> CreateExternalIdentityAsync(User user, IdentityType identityType, string externalValue);

        Task SendEmailConfirmationUrlAsync(string email, string verifyUrl);
    }
}