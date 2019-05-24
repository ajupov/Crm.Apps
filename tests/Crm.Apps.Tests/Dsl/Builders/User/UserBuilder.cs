using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crm.Clients.Users.Clients.Users;
using Crm.Clients.Users.Models;
using Crm.Common.UserContext;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Dsl.Builders.User
{
    public class UserBuilder : IUserBuilder
    {
        private readonly Clients.Users.Models.User _user;
        private readonly IUsersClient _usersClient;

        public UserBuilder(IUsersClient usersClient)
        {
            _usersClient = usersClient;
            _user = new Clients.Users.Models.User
            {
                AccountId = Guid.Empty,
                Surname = "Test",
                Name = "Test",
                Patronymic = "Test",
                BirthDate = DateTime.Today.AddYears(21),
                Gender = UserGender.Male,
                AvatarUrl = ""
            };
        }

        public UserBuilder WithAccountId(Guid accountId)
        {
            _user.AccountId = accountId;

            return this;
        }

        public UserBuilder WithSurname(string surname)
        {
            _user.Surname = surname;

            return this;
        }

        public UserBuilder WithName(string name)
        {
            _user.Name = name;

            return this;
        }

        public UserBuilder WithPatronymic(string patronymic)
        {
            _user.Patronymic = patronymic;

            return this;
        }

        public UserBuilder WithBirthDate(DateTime birthDate)
        {
            _user.BirthDate = birthDate;

            return this;
        }

        public UserBuilder WithGender(UserGender gender)
        {
            _user.Gender = gender;

            return this;
        }

        public UserBuilder WithAvatarUrl(string avatarUrl)
        {
            _user.AvatarUrl = avatarUrl;

            return this;
        }

        public UserBuilder AsLocked()
        {
            _user.IsLocked = true;

            return this;
        }

        public UserBuilder AsDeleted()
        {
            _user.IsDeleted = true;

            return this;
        }

        public UserBuilder WithSetting(string value)
        {
            if (_user.Settings == null)
            {
                _user.Settings = new List<UserSetting>();
            }

            _user.Settings.Add(new UserSetting
            {
                Type = UserSettingType.None,
                Value = value
            });

            return this;
        }

        public UserBuilder WithAttributeLink(Guid attributeId, string value)
        {
            if (_user.AttributeLinks == null)
            {
                _user.AttributeLinks = new List<UserAttributeLink>();
            }

            _user.AttributeLinks.Add(new UserAttributeLink
            {
                UserAttributeId = attributeId,
                Value = value
            });

            return this;
        }

        public UserBuilder WithGroupLink(Guid groupId)
        {
            if (_user.GroupLinks == null)
            {
                _user.GroupLinks = new List<UserGroupLink>();
            }

            _user.GroupLinks.Add(new UserGroupLink
            {
                UserGroupId = groupId
            });

            return this;
        }

        public UserBuilder WithPermission(Permission permission)
        {
            if (_user.Permissions == null)
            {
                _user.Permissions = new List<UserPermission>();
            }

            _user.Permissions.Add(new UserPermission
            {
                Permission = permission
            });

            return this;
        }

        public async Task<Clients.Users.Models.User> BuildAsync()
        {
            if (_user.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_user.AccountId));
            }

            var createdId = await _usersClient.CreateAsync(_user).ConfigureAwait(false);

            return await _usersClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}