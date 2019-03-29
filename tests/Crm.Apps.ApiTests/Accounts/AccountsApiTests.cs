using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crm.Apps.ApiTests.Accounts.Models;
using Crm.Utils.Http;
using Xunit;

namespace Crm.Apps.ApiTests.Accounts
{
    public class AccountsApiTests : BaseApiTests
    {
        private readonly string _host;

        public AccountsApiTests()
        {
            _host = Configuration["AccountsHost"];
        }

        [Fact]
        public Task Status()
        {
            return HttpClientFactory.GetAsync(_host);
        }

        [Fact]
        public async Task GetAccountSettingsTypes()
        {
            var types = await GetAccountSettingTypesAsync().ConfigureAwait(false);

            Assert.NotNull(types);
        }

        [Fact]
        public async Task Get()
        {
            var id = await CreateAccountAsync().ConfigureAwait(false);
            var account = await GetAccountAsync(id).ConfigureAwait(false);

            Assert.NotNull(account);
            Assert.NotEqual(id, account.Id);
        }
        
        [Fact]
        public async Task GetList()
        {
            var id1 = await CreateAccountAsync().ConfigureAwait(false);
            var id2 = await CreateAccountAsync().ConfigureAwait(false);
            
            var account = await GetAccountAsync(id).ConfigureAwait(false);

            Assert.NotNull(account);
            Assert.NotEqual(id, account.Id);
        }

        [Fact]
        public async Task Create()
        {
            var id = await CreateAccountAsync().ConfigureAwait(false);

            Assert.NotEqual(id, Guid.Empty);
        }
        
        

        private Task<ICollection<AccountSettingType>> GetAccountSettingTypesAsync()
        {
            return HttpClientFactory.GetAsync<ICollection<AccountSettingType>>($"{_host}/Api/Accounts/Settings");
        }

        private Task<Account> GetAccountAsync(Guid id)
        {
            return HttpClientFactory.GetAsync<Account>($"{_host}/Api/Accounts/Get", new {id});
        }

        private Task<ICollection<Account>> GetAccountsListAsync(ICollection<Guid> ids)
        {
            return HttpClientFactory.GetAsync<ICollection<Account>>($"{_host}/Api/Accounts/GetList", new {ids});
        }
        
        private Task<Guid> CreateAccountAsync()
        {
            return HttpClientFactory.PostAsync<Guid>($"{_host}/Api/Accounts/Create");
        }
    }
}