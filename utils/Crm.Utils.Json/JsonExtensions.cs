using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Crm.Utils.Json
{
    public static class JsonExtensions
    {
        public static T FromJsonString<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static string ToJsonString(this object value, bool ignoreChanges = false)
        {
            if (!ignoreChanges)
            {
                return JsonConvert.SerializeObject(value);
            }
            
            var jObject = JObject.FromObject(value);

            jObject["Changes"].Parent.Remove();

            return jObject.ToString();
        }
    }
}