using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crm.Apps.Clients.Users.Clients;
using Crm.Apps.Clients.Users.Models;
using Crm.Common.All.UserContext;

namespace Crm.Apps.Tests.Builders.Users
{
    public class UserBuilder : IUserBuilder
    {
        private readonly IUsersClient _usersClient;
        private readonly User _user;

        public UserBuilder(IUsersClient usersClient)
        {
            _usersClient = usersClient;
            _user = new User
            {
                AccountId = Guid.Empty,
                Surname = "Test",
                Name = "Test",
                Patronymic = "Test",
                BirthDate = DateTime.Today.AddYears(21),
                Gender = UserGender.Male,
                AvatarUrl = "",
                IsLocked = false,
                IsDeleted = false
            };
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
                Type = UserSettingType.IsDarkTheme,
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

        public UserBuilder WithRole(Role role)
        {
            if (_user.Roles == null)
            {
                _user.Roles = new List<UserRole>();
            }

            _user.Roles.Add(new UserRole
            {
                Role = role
            });

            return this;
        }

        public async Task<User> BuildAsync()
        {
            var id = await _usersClient.CreateAsync(_user);

            return await _usersClient.GetAsync(id);
        }
    }
}