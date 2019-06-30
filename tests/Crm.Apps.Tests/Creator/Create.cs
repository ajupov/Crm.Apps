using System;
using Crm.Apps.Tests.Builders.Accounts;
using Crm.Apps.Tests.Builders.Companies;
using Crm.Apps.Tests.Builders.Identities;
using Crm.Apps.Tests.Builders.Leads;
using Crm.Apps.Tests.Builders.Products;
using Crm.Apps.Tests.Builders.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Apps.Tests.Creator
{
    public class Create : ICreate
    {
        private readonly IServiceProvider _serviceCollection;

        public Create(IServiceProvider serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public IAccountBuilder Account => _serviceCollection.GetService<IAccountBuilder>();

        public IUserBuilder User => _serviceCollection.GetService<IUserBuilder>();

        public IUserAttributeBuilder UserAttribute => _serviceCollection.GetService<IUserAttributeBuilder>();

        public IUserGroupBuilder UserGroup => _serviceCollection.GetService<IUserGroupBuilder>();

        public IIdentityBuilder Identity => _serviceCollection.GetService<IIdentityBuilder>();

        public IIdentityTokenBuilder IdentityToken => _serviceCollection.GetService<IIdentityTokenBuilder>();

        public IProductBuilder Product => _serviceCollection.GetService<IProductBuilder>();

        public IProductCategoryBuilder ProductCategory => _serviceCollection.GetService<IProductCategoryBuilder>();

        public IProductStatusBuilder ProductStatus => _serviceCollection.GetService<IProductStatusBuilder>();

        public IProductAttributeBuilder ProductAttribute => _serviceCollection.GetService<IProductAttributeBuilder>();

        public ILeadBuilder Lead => _serviceCollection.GetService<ILeadBuilder>();

        public ILeadSourceBuilder LeadSource => _serviceCollection.GetService<ILeadSourceBuilder>();

        public ILeadAttributeBuilder LeadAttribute => _serviceCollection.GetService<ILeadAttributeBuilder>();

        public ILeadCommentBuilder LeadComment => _serviceCollection.GetService<ILeadCommentBuilder>();

        public ICompanyBuilder Company => _serviceCollection.GetService<ICompanyBuilder>();

        public ICompanyAttributeBuilder CompanyAttribute => _serviceCollection.GetService<ICompanyAttributeBuilder>();

        public ICompanyCommentBuilder CompanyComment => _serviceCollection.GetService<ICompanyCommentBuilder>();
    }
}