using System;
using System.Collections.Generic;
using Crm.Clients.Users.Models;
using Crm.Common.UserContext;

namespace Crm.Apps.Tests.Dsl.Builders
{
    public class UserBuilder
    {
        private readonly User _user;

        public UserBuilder(Guid accountId)
        {
            _user = new User
            {
                AccountId = accountId,
                Surname = "Test",
                Name = "Test",
                Patronymic = "Test",
                BirthDate = DateTime.Today.AddYears(21),
                Gender = UserGender.Male,
                AvatarUrl = ""
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
                AttributeId = attributeId,
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
                GroupId = groupId
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
        
        public User Build()
        {
            return _user;
        }
    }
}