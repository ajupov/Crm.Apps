using Crm.Apps.Tests.Dsl.Builders.Account;
using Crm.Apps.Tests.Dsl.Builders.Company;
using Crm.Apps.Tests.Dsl.Builders.CompanyAttribute;
using Crm.Apps.Tests.Dsl.Builders.CompanyComment;
using Crm.Apps.Tests.Dsl.Builders.Identity;
using Crm.Apps.Tests.Dsl.Builders.IdentityToken;
using Crm.Apps.Tests.Dsl.Builders.Lead;
using Crm.Apps.Tests.Dsl.Builders.LeadAttribute;
using Crm.Apps.Tests.Dsl.Builders.LeadComment;
using Crm.Apps.Tests.Dsl.Builders.LeadSource;
using Crm.Apps.Tests.Dsl.Builders.Product;
using Crm.Apps.Tests.Dsl.Builders.ProductAttribute;
using Crm.Apps.Tests.Dsl.Builders.ProductCategory;
using Crm.Apps.Tests.Dsl.Builders.ProductStatus;
using Crm.Apps.Tests.Dsl.Builders.User;
using Crm.Apps.Tests.Dsl.Builders.UserAttribute;
using Crm.Apps.Tests.Dsl.Builders.UserGroup;

namespace Crm.Apps.Tests.Dsl.Creator
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