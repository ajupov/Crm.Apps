using System;
using System.Collections.Generic;
using System.Linq;

namespace Crm.Utils.Enums
{
    public static class EnumsExtensions
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).OfType<T>();
        }

        public static Dictionary<T, string> GetAsDictionary<T>()
        {
            return GetValues<T>().ToDictionary(k => k, v => v.ToString());
        }
    }
}