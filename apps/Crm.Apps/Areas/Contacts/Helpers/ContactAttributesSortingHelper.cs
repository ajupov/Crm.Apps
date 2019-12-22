using System.Linq;
using Crm.Apps.Areas.Contacts.Models;

namespace Crm.Apps.Areas.Contacts.Helpers
{
    public static class ContactAttributesSortingHelper
    {
        public static IOrderedQueryable<ContactAttribute> Sort(this IQueryable<ContactAttribute> queryable,
            string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(ContactAttribute.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(ContactAttribute.Type):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Type)
                        : queryable.OrderBy(x => x.Type);
                case nameof(ContactAttribute.Key):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Key)
                        : queryable.OrderBy(x => x.Key);
                case nameof(ContactAttribute.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}