using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.String;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.RequestParameters;

namespace Crm.Apps.Areas.Deals.Helpers
{
    public static class DealsFiltersHelper
    {
        public static bool FilterByAdditional(this Deal product, DealGetPagedListRequestParameter request)
        {
            return (request.TypeIds == null || !request.TypeIds.Any() ||
                    request.TypeIds.Any(x => TypeIdsPredicate(product, x))) &&
                   (request.StatusIds == null || !request.StatusIds.Any() ||
                    request.StatusIds.Any(x => StatusIdsPredicate(product, x))) &&
                   (request.CompanyIds == null || !request.CompanyIds.Any() ||
                    request.CompanyIds.Any(x => CompanyIdsPredicate(product, x))) &&
                   (request.ContactIds == null || !request.ContactIds.Any() ||
                    request.ContactIds.Any(x => ContactIdsPredicate(product, x))) &&
                   (request.CreateUserIds == null || !request.CreateUserIds.Any() ||
                    request.CreateUserIds.Any(x => CreateUserIdsPredicate(product, x))) &&
                   (request.ResponsibleUserIds == null || !request.ResponsibleUserIds.Any() ||
                    request.ResponsibleUserIds.Any(x => ResponsibleUserIdsPredicate(product, x))) &&
                   (request.Attributes == null || !request.Attributes.Any() ||
                    (request.AllAttributes is false
                        ? request.Attributes.Any(x => AttributePredicate(product, x))
                        : request.Attributes.All(x => AttributePredicate(product, x)))) &&
                   (request.PositionsProductIds == null || !request.PositionsProductIds.Any() ||
                    request.PositionsProductIds.Any(x => PositionsProductIdsPredicate(product, x)));
        }

        private static bool TypeIdsPredicate(Deal product, Guid id)
        {
            return product.TypeId == id;
        }

        private static bool StatusIdsPredicate(Deal product, Guid id)
        {
            return product.StatusId == id;
        }

        private static bool CompanyIdsPredicate(Deal product, Guid id)
        {
            return product.CompanyId == id;
        }

        private static bool ContactIdsPredicate(Deal product, Guid id)
        {
            return product.ContactId == id;
        }

        private static bool CreateUserIdsPredicate(Deal product, Guid id)
        {
            return product.CreateUserId == id;
        }

        private static bool ResponsibleUserIdsPredicate(Deal product, Guid id)
        {
            return product.ResponsibleUserId == id;
        }

        private static bool AttributePredicate(Deal product, KeyValuePair<Guid, string> pair)
        {
            var (key, value) = pair;

            return product.AttributeLinks != null && product.AttributeLinks.Any(x =>
                       x.DealAttributeId == key && (value.IsEmpty() || x.Value == value));
        }

        private static bool PositionsProductIdsPredicate(Deal product, Guid id)
        {
            return product.Positions == null || !product.Positions.Any() ||
                   product.Positions.Any(x => x.ProductId == id);
        }
    }
}