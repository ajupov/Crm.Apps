using System;
using System.Collections.Generic;
using Crm.Clients.Users.Models;
using Crm.Common.UserContext;

namespace Crm.Apps.Tests.Dsl.Builders
{
    public class UserGroupBuilder
    {
        private readonly UserGroup _userGroup;

        public UserGroupBuilder(Guid accountId)
        {
            _userGroup = new UserGroup
            {
                AccountId = accountId,
                Name = "test"
            };
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

        public UserGroup Build()
        {
            return _userGroup;
        }
    }
}