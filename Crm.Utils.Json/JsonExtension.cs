using Newtonsoft.Json;

namespace Crm.Utils.Json
{
    public static class JsonExtension
    {
        public static T FromJsonString<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static string ToJsonString(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}