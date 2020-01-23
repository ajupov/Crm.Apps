using System;
using Crm.Apps.Tests.Builders.Activities;
using Crm.Apps.Tests.Builders.Companies;
using Crm.Apps.Tests.Builders.Contacts;
using Crm.Apps.Tests.Builders.Deals;
using Crm.Apps.Tests.Builders.Leads;
using Crm.Apps.Tests.Builders.OAuth;
using Crm.Apps.Tests.Builders.Products;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Apps.Tests.Services.Creator
{
    public class Create : ICreate
    {
        private readonly IServiceProvider _services;

        public Create(IServiceProvider services)
        {
            _services = services;
        }

        public IOAuthBuilder OAuth => _services.GetService<IOAuthBuilder>();

        public IProductBuilder Product => _services.GetService<IProductBuilder>();

        public IProductCategoryBuilder ProductCategory => _services.GetService<IProductCategoryBuilder>();

        public IProductStatusBuilder ProductStatus => _services.GetService<IProductStatusBuilder>();

        public IProductAttributeBuilder ProductAttribute => _services.GetService<IProductAttributeBuilder>();

        public ILeadBuilder Lead => _services.GetService<ILeadBuilder>();

        public ILeadSourceBuilder LeadSource => _services.GetService<ILeadSourceBuilder>();

        public ILeadAttributeBuilder LeadAttribute => _services.GetService<ILeadAttributeBuilder>();

        public ILeadCommentBuilder LeadComment => _services.GetService<ILeadCommentBuilder>();

        public ICompanyBuilder Company => _services.GetService<ICompanyBuilder>();

        public ICompanyAttributeBuilder CompanyAttribute => _services.GetService<ICompanyAttributeBuilder>();

        public ICompanyCommentBuilder CompanyComment => _services.GetService<ICompanyCommentBuilder>();

        public IContactBuilder Contact => _services.GetService<IContactBuilder>();

        public IContactAttributeBuilder ContactAttribute => _services.GetService<IContactAttributeBuilder>();

        public IContactCommentBuilder ContactComment => _services.GetService<IContactCommentBuilder>();

        public IDealBuilder Deal => _services.GetService<IDealBuilder>();

        public IDealStatusBuilder DealStatus => _services.GetService<IDealStatusBuilder>();

        public IDealTypeBuilder DealType => _services.GetService<IDealTypeBuilder>();

        public IDealAttributeBuilder DealAttribute => _services.GetService<IDealAttributeBuilder>();

        public IDealCommentBuilder DealComment => _services.GetService<IDealCommentBuilder>();

        public IActivityBuilder Activity => _services.GetService<IActivityBuilder>();

        public IActivityStatusBuilder ActivityStatus => _services.GetService<IActivityStatusBuilder>();

        public IActivityTypeBuilder ActivityType => _services.GetService<IActivityTypeBuilder>();

        public IActivityAttributeBuilder ActivityAttribute => _services.GetService<IActivityAttributeBuilder>();

        public IActivityCommentBuilder ActivityComment => _services.GetService<IActivityCommentBuilder>();
    }
}