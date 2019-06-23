using System;
using Crm.Apps.Tests.Dsl.Builders.Account;
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
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Apps.Tests.Dsl.Creator
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
    }
}