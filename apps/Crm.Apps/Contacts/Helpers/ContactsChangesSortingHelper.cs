using System.Linq;
using Crm.Apps.Contacts.Models;

namespace Crm.Apps.Contacts.Helpers
{
    public static class ContactsChangesSortingHelper
    {
        public static IOrderedQueryable<ContactChange> Sort(this IQueryable<ContactChange> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(ContactChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(ContactChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}