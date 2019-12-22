using System.Linq;
using Crm.Apps.Areas.Contacts.Models;

namespace Crm.Apps.Areas.Contacts.Helpers
{
    public static class ContactAttributesChangesSortingHelper
    {
        public static IOrderedQueryable<ContactAttributeChange> Sort(this IQueryable<ContactAttributeChange> queryable,
            string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(ContactAttributeChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(ContactAttributeChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}