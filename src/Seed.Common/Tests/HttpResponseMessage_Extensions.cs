using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Seed.Common.Exceptions;

namespace Seed.Common.Tests
{
    public static class HttpResponseMessage_Extensions
    {
        public static async Task<string> GetApiResponseDataAsync(this HttpResponseMessage msg)
        {
            return (await msg.GetApiResponsePropertyAsync("data")).ToString();
        }

        public static async Task<string> GetApiResponseErrorAsync(this HttpResponseMessage msg)
        {
            return (await msg.GetApiResponsePropertyAsync("error")).ToString();
        }

        public static async Task<TException> GetApiResponseExceptionAsync<TException>(this HttpResponseMessage msg)
            where TException : ApiException
        {
            var content = (await msg.GetApiResponsePropertyAsync("error")).ToString();

            return JsonConvert.DeserializeObject<TException>(content);
        }

        public static async Task<T> ParseApiResponseDataPropertyAsync<T>(this HttpResponseMessage msg,
            string propertyPath) where T : JToken
        {
            return (await msg.GetApiResponsePropertyAsync("data")).Value<T>();
        }

        public static async Task<JToken> GetApiResponsePropertyAsync(this HttpResponseMessage msg, string property)
        {
            var rawContent = await msg.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(rawContent))
                throw new Exception("Response contains no content");

            var content = JObject.Parse(rawContent);

            if (content.TryGetValue(property, out var propertyToken))
                return propertyToken.ToString();

            throw new Exception($"Unexpected response content: {content}");
        }
    }
}