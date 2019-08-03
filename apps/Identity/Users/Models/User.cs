using System;

namespace Identity.Users.Models
{
    public class User
    {
        public User(
            string surname,
            string name,
            DateTime? birthDate,
            UserGender? gender,
            string avatarUrl)
        {
            Id = Guid.NewGuid();
            Surname = surname;
            Name = name;
            BirthDate = birthDate;
            Gender = gender;
            AvatarUrl = avatarUrl;
            IsLocked = false;
            IsDeleted = false;
            CreateDateTime = DateTime.UtcNow;
        }

        public Guid Id { get; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public DateTime? BirthDate { get; set; }

        public UserGender? Gender { get; set; }

        public string AvatarUrl { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; }
    }
}