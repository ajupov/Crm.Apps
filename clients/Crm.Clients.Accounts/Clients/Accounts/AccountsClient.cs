using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;

namespace Crm.Clients.Accounts.Clients.Accounts
{
    public class AccountsClient : HttpClient, IAccountsClient
    {
        public AccountsClient(string host) : base($"{host}/api/v1/Accounts")
        {
        }

        public Task<Account> GetAsync(Guid id, CancellationToken ct = default)
        {
            return GetAsync<Account>("Get", new {id});
        }

        public Task<List<Account>> GetListAsync(params int[] ids)
        {
            return PostAsync<List<Account>>("GetList", new {ids});
        }

        public Task<List<Account>> GetListAsync(
            bool? isLocked = null,
            bool? isDeleted = null,
            DateTime? minCreateDate = null,
            DateTime? maxCreateDate = null,
            int offset = 0,
            int limit = 10,
            string sortBy = null,
            string orderBy = null)
        {
            return GetAsync<List<Account>>("GetList",
                new {isLocked, isDeleted, minCreateDate, maxCreateDate, offset, limit, sortBy, orderBy});
        }

        public Task<int> CreateAsync(int changerUserId)
        {
            return PostAsync<int>("Create", new {changerUserId});
        }

        public Task LockAsync(int changerUserId, params int[] ids)
        {
            return PostAsync("Lock", new {changerUserId, ids});
        }

        public Task UnlockAsync(int changerUserId, params int[] ids)
        {
            return PostAsync("Unlock", new {changerUserId, ids});
        }

        public Task DeleteAsync(int changerUserId, params int[] ids)
        {
            return PostAsync("Delete", new {changerUserId, ids});
        }

        public Task RestoreAsync(int changerUserId, params int[] ids)
        {
            return PostAsync("Restore", new {changerUserId, ids});
        }
    }
}