using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crm.Clients.Users.Clients;
using Crm.Clients.Users.Models;
using Crm.Common.UserContext;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Users
{
    public class UserGroupBuilder : IUserGroupBuilder
    {
        private readonly Clients.Users.Models.UserGroup _userGroup;
        private readonly IUserGroupsClient _userGroupsClient;

        public UserGroupBuilder(IUserGroupsClient userGroupsClient)
        {
            _userGroupsClient = userGroupsClient;
            _userGroup = new Clients.Users.Models.UserGroup
            {
                AccountId = Guid.Empty,
                Name = "Test"
            };
        }

        public UserGroupBuilder WithAccountId(Guid accountId)
        {
            _userGroup.AccountId = accountId;

            return this;
        }

        public UserGroupBuilder WithName(string name)
        {
            _userGroup.Name = name;

            return this;
        }

        public UserGroupBuilder WithPermission(Permission permission)
        {
            if (_userGroup.Permissions == null)
            {
                _userGroup.Permissions = new List<UserGroupPermission>();
            }

            _userGroup.Permissions.Add(new UserGroupPermission
            {
                Permission = permission
            });

            return this;
        }

        public async Task<Clients.Users.Models.UserGroup> BuildAsync()
        {
            if (_userGroup.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_userGroup.AccountId));
            }

            var createdId = await _userGroupsClient.CreateAsync(_userGroup).ConfigureAwait(false);

            return await _userGroupsClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}