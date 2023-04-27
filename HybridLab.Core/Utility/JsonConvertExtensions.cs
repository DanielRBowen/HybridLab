using Newtonsoft.Json;

namespace HybridLab.Core.Utility
{
    public static class JsonConvertHelper
    {
        public static T TryDeserializeObject<T>(string json, T defaultValue = default)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonException)
            {
                return defaultValue;
            }
        }
    }
}
