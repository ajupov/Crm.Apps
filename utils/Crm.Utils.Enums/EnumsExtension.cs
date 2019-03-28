using System;
using System.Linq;

namespace Crm.Utils.Enums
{
    public static class EnumsExtension
    {
        public static T[] GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).OfType<T>().ToArray();
        }
    }
}