using System;
using Identity.Users.Models;

namespace Identity.Users.Parameters
{
    public class UserGetPagedListParameter
    {
        public UserGetPagedListParameter(
            string surname = default,
            string name = default,
            DateTime? minBirthDate = default,
            DateTime? maxBirthDate = default,
            UserGender? gender = default,
            bool? isLocked = false,
            bool? isDeleted = false,
            DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default,
            int offset = default,
            int limit = 10,
            string sortBy = "CreateDateTime",
            string orderBy = "desc")
        {
            Surname = surname;
            Name = name;
            MinBirthDate = minBirthDate;
            MaxBirthDate = maxBirthDate;
            Gender = gender;
            IsLocked = isLocked;
            IsDeleted = isDeleted;
            MinCreateDate = minCreateDate;
            MaxCreateDate = maxCreateDate;
            Offset = offset;
            Limit = limit;
            SortBy = sortBy;
            OrderBy = orderBy;
        }

        public string Surname { get; }

        public string Name { get; }

        public DateTime? MinBirthDate { get; }

        public DateTime? MaxBirthDate { get; }

        public UserGender? Gender { get; }

        public bool? IsLocked { get; }

        public bool? IsDeleted { get; }

        public DateTime? MinCreateDate { get; }

        public DateTime? MaxCreateDate { get; }

        public int Offset { get; }

        public int Limit { get; }

        public string SortBy { get; }

        public string OrderBy { get; }
    }
}