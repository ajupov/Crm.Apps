using System.Collections.Generic;
using Crm.Clients.Accounts.Models;

namespace Crm.Apps.Tests.Dsl.Builders
{
    public class AccountBuilder
    {
        private readonly Account _account;

        public AccountBuilder()
        {
            _account = new Account();
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

        public AccountBuilder WithSetting(string value)
        {
            if (_account.Settings == null)
            {
                _account.Settings = new List<AccountSetting>();
            }

            _account.Settings.Add(new AccountSetting
            {
                Type = AccountSettingType.None,
                Value = value
            });

            return this;
        }

        public Account Build()
        {
            return _account;
        }
    }
}