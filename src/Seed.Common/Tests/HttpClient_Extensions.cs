using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Seed.Common.Tests
{
    public static class HttpClient_Extensions
    {
        public static Task<HttpResponseMessage> PostObjectAsync(this HttpClient instance, string url, object payload)
        {
            var content = new StringContent(JsonConvert.SerializeObject(payload));

            return instance.PostAsync(url, content);
        }
    }
}