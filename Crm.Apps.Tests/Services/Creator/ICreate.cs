using Crm.Apps.Tests.Builders.Activities;
using Crm.Apps.Tests.Builders.Companies;
using Crm.Apps.Tests.Builders.Contacts;
using Crm.Apps.Tests.Builders.Deals;
using Crm.Apps.Tests.Builders.Leads;
using Crm.Apps.Tests.Builders.Products;

namespace Crm.Apps.Tests.Services.Creator
{
    public interface ICreate
    {
        IProductBuilder Product { get; }

        IProductCategoryBuilder ProductCategory { get; }

        IProductStatusBuilder ProductStatus { get; }

        IProductAttributeBuilder ProductAttribute { get; }

        ILeadBuilder Lead { get; }

        ILeadSourceBuilder LeadSource { get; }

        ILeadAttributeBuilder LeadAttribute { get; }

        ILeadCommentBuilder LeadComment { get; }

        ICompanyBuilder Company { get; }

        ICompanyAttributeBuilder CompanyAttribute { get; }

        ICompanyCommentBuilder CompanyComment { get; }

        IContactBuilder Contact { get; }

        IContactAttributeBuilder ContactAttribute { get; }

        IContactCommentBuilder ContactComment { get; }

        IDealBuilder Deal { get; }

        IDealStatusBuilder DealStatus { get; }

        IDealTypeBuilder DealType { get; }

        IDealAttributeBuilder DealAttribute { get; }

        IDealCommentBuilder DealComment { get; }

        IActivityBuilder Activity { get; }

        IActivityStatusBuilder ActivityStatus { get; }

        IActivityTypeBuilder ActivityType { get; }

        IActivityAttributeBuilder ActivityAttribute { get; }

        IActivityCommentBuilder ActivityComment { get; }
    }
}