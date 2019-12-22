using System.Linq;
using Crm.Apps.Areas.Contacts.Models;

namespace Crm.Apps.Areas.Contacts.Helpers
{
    public static class ContactsSortingHelper
    {
        public static IOrderedQueryable<Contact> Sort(this IQueryable<Contact> queryable, string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(Contact.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(Contact.Surname):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Surname)
                        : queryable.OrderBy(x => x.Surname);
                case nameof(Contact.Name):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Name)
                        : queryable.OrderBy(x => x.Name);
                case nameof(Contact.Patronymic):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Patronymic)
                        : queryable.OrderBy(x => x.Patronymic);
                case nameof(Contact.Phone):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Phone)
                        : queryable.OrderBy(x => x.Phone);
                case nameof(Contact.Email):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Email)
                        : queryable.OrderBy(x => x.Email);
                case nameof(Contact.TaxNumber):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.TaxNumber)
                        : queryable.OrderBy(x => x.TaxNumber);
                case nameof(Contact.Post):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Post)
                        : queryable.OrderBy(x => x.Post);
                case nameof(Contact.Postcode):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Postcode)
                        : queryable.OrderBy(x => x.Postcode);
                case nameof(Contact.Country):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Country)
                        : queryable.OrderBy(x => x.Country);
                case nameof(Contact.Region):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Region)
                        : queryable.OrderBy(x => x.Region);
                case nameof(Contact.Province):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Province)
                        : queryable.OrderBy(x => x.Province);
                case nameof(Contact.City):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.City)
                        : queryable.OrderBy(x => x.City);
                case nameof(Contact.Street):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Street)
                        : queryable.OrderBy(x => x.Street);
                case nameof(Contact.House):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.House)
                        : queryable.OrderBy(x => x.House);
                case nameof(Contact.Apartment):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Apartment)
                        : queryable.OrderBy(x => x.Apartment);
                case nameof(Contact.BirthDate):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.BirthDate)
                        : queryable.OrderBy(x => x.BirthDate);
                case nameof(Contact.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}