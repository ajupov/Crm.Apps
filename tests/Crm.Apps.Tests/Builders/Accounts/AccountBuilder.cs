using System.Collections.Generic;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Clients;
using Crm.Clients.Accounts.Models;
using Crm.Clients.Accounts.RequestParameters;

namespace Crm.Apps.Tests.Builders.Accounts
{
    public class AccountBuilder : IAccountBuilder
    {
        private readonly IAccountsClient _accountsClient;

        private AccountType _type;
        private bool _isLocked;
        private bool _isDeleted;
        private List<AccountSetting> _settings;

        public AccountBuilder(IAccountsClient accountsClient)
        {
            _accountsClient = accountsClient;
        }

        public AccountBuilder WithType(AccountType type)
        {
            _type = type;

            return this;
        }

        public AccountBuilder AsLocked()
        {
            _isLocked = true;

            return this;
        }

        public AccountBuilder AsDeleted()
        {
            _isDeleted = true;

            return this;
        }

        public AccountBuilder WithSetting(AccountSettingType type, string value = null)
        {
            if (_settings == null)
            {
                _settings = new List<AccountSetting>();
            }

            var setting = new AccountSetting
            {
                Type = type,
                Value = value
            };

            _settings.Add(setting);

            return this;
        }

        public async Task<Account> BuildAsync()
        {
            var request = new AccountCreateRequest
            {
                Type = _type,
                IsLocked = _isLocked,
                IsDeleted = _isDeleted,
                Settings = _settings?.ToArray()
            };

            var createdId = await _accountsClient.CreateAsync(request);

            return await _accountsClient.GetAsync(createdId);
        }
    }
}