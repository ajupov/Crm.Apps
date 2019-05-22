using System;
using Crm.Apps.Tests.Dsl.Builders.Account;
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
    }
}