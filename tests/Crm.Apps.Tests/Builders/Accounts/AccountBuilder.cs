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
        private readonly AccountCreateRequest _request;

        public AccountBuilder(IAccountsClient accountsClient)
        {
            _accountsClient = accountsClient;
            _request = new AccountCreateRequest
            {
                Type = AccountType.MlmSystem,
                IsLocked = false,
                IsDeleted = false,
                Settings = null
            };
        }

        public AccountBuilder WithType(AccountType type)
        {
            _request.Type = type;

            return this;
        }

        public AccountBuilder AsLocked()
        {
            _request.IsLocked = true;

            return this;
        }

        public AccountBuilder AsDeleted()
        {
            _request.IsDeleted = true;

            return this;
        }

        public AccountBuilder WithSetting(AccountSettingType type, string? value = null)
        {
            if (_request.Settings == null)
            {
                _request.Settings = new List<AccountSetting>();
            }

            var setting = new AccountSetting
            {
                Type = type,
                Value = value
            };

            _request.Settings.Add(setting);

            return this;
        }

        public async Task<Account> BuildAsync()
        {
            var createdId = await _accountsClient.CreateAsync(_request);

            return await _accountsClient.GetAsync(createdId);
        }
    }
}