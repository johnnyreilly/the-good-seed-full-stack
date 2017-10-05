using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Seed.Api.Tests.Infrastructure
{
    public static class HttpClient_Extensions
    {
        public static Task<HttpResponseMessage> PostObjectAsync(this HttpClient instance, string url, object payload)
        {
            var content = new StringContent(JsonConvert.SerializeObject(payload));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            return instance.PostAsync(url, content);
        }
    }
}