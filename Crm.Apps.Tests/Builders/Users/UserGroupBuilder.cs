using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crm.Apps.Clients.Users.Clients;
using Crm.Apps.Clients.Users.Models;
using Crm.Common.All.UserContext;

namespace Crm.Apps.Tests.Builders.Users
{
    public class UserGroupBuilder : IUserGroupBuilder
    {
        private readonly IUserGroupsClient _userGroupsClient;
        private readonly UserGroup _group;

        public UserGroupBuilder(IUserGroupsClient userGroupsClient)
        {
            _userGroupsClient = userGroupsClient;
            _group = new UserGroup
            {
                AccountId = Guid.Empty,
                Name = "Test",
                IsDeleted = false
            };
        }

        public UserGroupBuilder WithName(string name)
        {
            _group.Name = name;

            return this;
        }


        public UserGroupBuilder AsDeleted()
        {
            _group.IsDeleted = true;

            return this;
        }

        public UserGroupBuilder WithRole(Role role)
        {
            if (_group.Roles == null)
            {
                _group.Roles = new List<UserGroupRole>();
            }

            _group.Roles.Add(new UserGroupRole
            {
                Role = role
            });

            return this;
        }


        public async Task<UserGroup> BuildAsync()
        {
            var id = await _userGroupsClient.CreateAsync(_group);

            return await _userGroupsClient.GetAsync(id);
        }
    }
}