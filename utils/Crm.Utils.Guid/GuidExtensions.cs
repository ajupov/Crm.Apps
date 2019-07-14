using System.Collections.Generic;
using System.Linq;

namespace Crm.Utils.Guid
{
    public static class GuidExtensions
    {
        public static bool IsEmpty(this System.Guid? value)
        {
            return !value.HasValue || IsEmpty(value.Value);
        }

        public static bool IsEmpty(this System.Guid value)
        {
            return value == System.Guid.Empty;
        }

        public static bool IsEmpty(this IEnumerable<System.Guid?> values)
        {
            return values == null || values.All(IsEmpty);
        }

        public static bool IsEmpty(this IEnumerable<System.Guid> values)
        {
            return values == null || values.All(IsEmpty);
        }
    }
}