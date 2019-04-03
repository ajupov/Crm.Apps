using System;
using Xunit.Sdk;

namespace Crm.Infrastructure.DependencyInjection.Tests.Attributes
{
    [TestFrameworkDiscoverer("Xunit.Sdk.TestFrameworkTypeDiscoverer", "xunit.execution.{Platform}")]
    [AttributeUsage(AttributeTargets.Assembly)]
    public class DependencyInjectAttribute : Attribute, ITestFrameworkAttribute
    {
        public DependencyInjectAttribute(string typeName, string assemblyName)
        {
        }
    }
}