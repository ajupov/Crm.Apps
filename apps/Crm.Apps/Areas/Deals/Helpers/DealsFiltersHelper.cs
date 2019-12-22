using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.String;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.Parameters;

namespace Crm.Apps.Areas.Deals.Helpers
{
    public static class DealsFiltersHelper
    {
        public static bool FilterByAdditional(this Deal product, DealGetPagedListParameter parameter)
        {
            return (parameter.TypeIds == null || !parameter.TypeIds.Any() ||
                    parameter.TypeIds.Any(x => TypeIdsPredicate(product, x))) &&
                   (parameter.StatusIds == null || !parameter.StatusIds.Any() ||
                    parameter.StatusIds.Any(x => StatusIdsPredicate(product, x))) &&
                   (parameter.CompanyIds == null || !parameter.CompanyIds.Any() ||
                    parameter.CompanyIds.Any(x => CompanyIdsPredicate(product, x))) &&
                   (parameter.ContactIds == null || !parameter.ContactIds.Any() ||
                    parameter.ContactIds.Any(x => ContactIdsPredicate(product, x))) &&
                   (parameter.CreateUserIds == null || !parameter.CreateUserIds.Any() ||
                    parameter.CreateUserIds.Any(x => CreateUserIdsPredicate(product, x))) &&
                   (parameter.ResponsibleUserIds == null || !parameter.ResponsibleUserIds.Any() ||
                    parameter.ResponsibleUserIds.Any(x => ResponsibleUserIdsPredicate(product, x))) &&
                   (parameter.Attributes == null || !parameter.Attributes.Any() ||
                    (parameter.AllAttributes is false
                        ? parameter.Attributes.Any(x => AttributePredicate(product, x))
                        : parameter.Attributes.All(x => AttributePredicate(product, x)))) &&
                   (parameter.PositionsProductIds == null || !parameter.PositionsProductIds.Any() ||
                    parameter.PositionsProductIds.Any(x => PositionsProductIdsPredicate(product, x)));
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