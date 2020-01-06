using System.Collections.Generic;
using System.Threading.Tasks;
using Crm.Apps.Clients.Accounts.Clients;
using Crm.Apps.Clients.Accounts.Models;

namespace Crm.Apps.Tests.Builders.Accounts
{
    public class AccountBuilder : IAccountBuilder
    {
        private readonly IAccountsClient _accountsClient;
        private readonly Account _account;

        public AccountBuilder(IAccountsClient accountsClient)
        {
            _accountsClient = accountsClient;
            _account = new Account
            {
                Type = AccountType.MlmSystem,
                IsLocked = false,
                IsDeleted = false
            };
        }

        public AccountBuilder WithType(AccountType type)
        {
            _account.Type = type;

            return this;
        }

        public AccountBuilder AsLocked()
        {
            _account.IsLocked = true;

            return this;
        }

        public AccountBuilder AsDeleted()
        {
            _account.IsDeleted = true;

            return this;
        }

        public AccountBuilder WithSetting(AccountSettingType type, string value = null)
        {
            if (_account.Settings == null)
            {
                _account.Settings = new List<AccountSetting>();
            }

            var setting = new AccountSetting
            {
                Type = type,
                Value = value
            };

            _account.Settings.Add(setting);

            return this;
        }

        public async Task<Account> BuildAsync()
        {
            var id = await _accountsClient.CreateAsync(_account);

            return await _accountsClient.GetAsync(id);
        }
    }
}