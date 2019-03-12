using System.Linq;
using System.Reflection;

namespace Crm.Utils.DisplayAttribute
{
    public static class DisplayAttributeExtension
    {
        public static string GetDisplayName<TEnum>(this TEnum @enum)
        {
            return GetAttribute(@enum).GetName();
        }

        public static string GetDisplayShortName<TEnum>(this TEnum @enum)
        {
            return GetAttribute(@enum).GetShortName();
        }

        public static string GetDisplayGroupName<TEnum>(this TEnum @enum)
        {
            return GetAttribute(@enum).GetGroupName();
        }

        public static string GetDisplayDescription<TEnum>(this TEnum @enum)
        {
            return GetAttribute(@enum).GetDescription();
        }

        private static System.ComponentModel.DataAnnotations.DisplayAttribute GetAttribute<TEnum>(TEnum @enum)
        {
            return @enum.GetType().GetMember(@enum.ToString()).First()
                .GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>();
        }
    }
}