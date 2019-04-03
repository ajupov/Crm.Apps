using System;
using Xunit.Sdk;

namespace Crm.Infrastructure.Di
{
    [TestFrameworkDiscoverer("Xunit.Sdk.TestFrameworkTypeDiscoverer", "xunit.execution.{Platform}")]
    [AttributeUsage(AttributeTargets.Assembly)]
    public class DiAttribute : Attribute, ITestFrameworkAttribute
    {
        public DiAttribute(string typeName, string assemblyName)
        {
        }
    }
}