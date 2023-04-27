using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace HybridLab.Core.Utility
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<string> GetContentTypeName(this HttpResponseMessage response, IEnumerable<Type> types)
        {
            string content = await response.Content.ReadAsStringAsync();
            foreach (Type type in types)
            {
                try
                {
                    object obj = JsonConvert.DeserializeObject(content, type);
                    return type.GetType().Name;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return typeof(string).GetType().Name;
        }

        public static async Task<bool> IsContentJson(this HttpResponseMessage response)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                JObject.Parse(content);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

            //if (response.Content == null)
            //    return false;

            //if (response.Content.Headers.ContentType == null)
            //    return false;

            //return response.Content.Headers.ContentType.MediaType == "application/json" ? true : false;
        }
    }
}
