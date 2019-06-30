using System;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;
using Crm.Common.UserContext;

namespace Crm.Apps.Tests.Builders.Users
{
    public interface IUserBuilder
    {
        UserBuilder WithAccountId(Guid accountId);

        UserBuilder WithSurname(string surname);

        UserBuilder WithName(string name);

        UserBuilder WithPatronymic(string patronymic);

        UserBuilder WithBirthDate(DateTime birthDate);

        UserBuilder WithGender(UserGender gender);

        UserBuilder WithAvatarUrl(string avatarUrl);

        UserBuilder AsLocked();

        UserBuilder AsDeleted();

        UserBuilder WithSetting(string value);

        UserBuilder WithAttributeLink(Guid attributeId, string value);

        UserBuilder WithGroupLink(Guid groupId);

        UserBuilder WithPermission(Permission permission);

        Task<User> BuildAsync();
    }
}