using System;

namespace Crm.Apps.Tests.Extensions
{
    public static class ValueWithGuidExtension
    {
        public static string WithGuid(this string value)
        {
            return $"{value}-{Guid.NewGuid()}";
        }
    }
}