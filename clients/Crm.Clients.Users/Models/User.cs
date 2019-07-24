using System;
using System.Collections.Generic;

namespace Crm.Clients.Users.Models
{
    public class User
    {
        public User(
            Guid accountId,
            string surname,
            string name,
            string patronymic,
            DateTime? birthDate,
            UserGender gender,
            string avatarUrl = default,
            List<UserAttributeLink> attributeLinks = default,
            List<UserPermission> permissions = default,
            List<UserGroupLink> groupLinks = default,
            List<UserSetting> settings = default)
        {
            Id = Guid.NewGuid();
            AccountId = accountId;
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            BirthDate = birthDate;
            Gender = gender;
            AvatarUrl = avatarUrl;
            IsLocked = false;
            IsDeleted = false;
            CreateDateTime = DateTime.UtcNow;
            AttributeLinks = attributeLinks ?? new List<UserAttributeLink>();
            Permissions = permissions ?? new List<UserPermission>();
            GroupLinks = groupLinks ?? new List<UserGroupLink>();
            Settings = settings ?? new List<UserSetting>();
        }

        public Guid Id { get; }

        public Guid AccountId { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public DateTime? BirthDate { get; set; }

        public UserGender Gender { get; set; }

        public string AvatarUrl { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; }

        public List<UserAttributeLink> AttributeLinks { get; set; }

        public List<UserPermission> Permissions { get; set; }

        public List<UserGroupLink> GroupLinks { get; set; }

        public List<UserSetting> Settings { get; set; }
    }
}