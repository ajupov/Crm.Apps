using Crm.Apps.Tests.Builders.Accounts;
using Crm.Apps.Tests.Builders.Companies;
using Crm.Apps.Tests.Builders.Identities;
using Crm.Apps.Tests.Builders.Leads;
using Crm.Apps.Tests.Builders.Products;
using Crm.Apps.Tests.Builders.Users;

namespace Crm.Apps.Tests.Creator
{
    public interface ICreate
    {
        IAccountBuilder Account { get; }

        IUserBuilder User { get; }

        IUserAttributeBuilder UserAttribute { get; }

        IUserGroupBuilder UserGroup { get; }

        IIdentityBuilder Identity { get; }

        IIdentityTokenBuilder IdentityToken { get; }

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
    }
}