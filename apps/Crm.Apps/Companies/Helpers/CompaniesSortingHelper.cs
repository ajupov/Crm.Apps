using System.Linq;
using Crm.Apps.Leads.Models;

namespace Crm.Apps.Leads.Helpers
{
    public static class LeadsSortingHelper
    {
        public static IOrderedQueryable<Lead> Sort(this IQueryable<Lead> queryable, string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(Lead.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(Lead.Surname):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Surname)
                        : queryable.OrderBy(x => x.Surname);
                case nameof(Lead.Name):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Name)
                        : queryable.OrderBy(x => x.Name);
                case nameof(Lead.Patronymic):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Patronymic)
                        : queryable.OrderBy(x => x.Patronymic);
                case nameof(Lead.Phone):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Phone)
                        : queryable.OrderBy(x => x.Phone);
                case nameof(Lead.Email):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Email)
                        : queryable.OrderBy(x => x.Email);
                case nameof(Lead.CompanyName):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CompanyName)
                        : queryable.OrderBy(x => x.CompanyName);
                case nameof(Lead.Post):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Post)
                        : queryable.OrderBy(x => x.Post);
                case nameof(Lead.Postcode):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Postcode)
                        : queryable.OrderBy(x => x.Postcode);
                case nameof(Lead.Country):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Country)
                        : queryable.OrderBy(x => x.Country);
                case nameof(Lead.Region):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Region)
                        : queryable.OrderBy(x => x.Region);
                case nameof(Lead.Province):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Province)
                        : queryable.OrderBy(x => x.Province);
                case nameof(Lead.City):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.City)
                        : queryable.OrderBy(x => x.City);
                case nameof(Lead.Street):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Street)
                        : queryable.OrderBy(x => x.Street);
                case nameof(Lead.House):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.House)
                        : queryable.OrderBy(x => x.House);
                case nameof(Lead.Apartment):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Apartment)
                        : queryable.OrderBy(x => x.Apartment);
                case nameof(Lead.OpportunitySum):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.OpportunitySum)
                        : queryable.OrderBy(x => x.OpportunitySum);
                case nameof(Lead.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}