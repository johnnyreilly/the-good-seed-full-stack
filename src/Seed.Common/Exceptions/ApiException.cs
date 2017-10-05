using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Seed.Common.Exceptions
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class ApiException : Exception
    {
        // C'tor
        protected ApiException(HttpStatusCode responseStatusCode, ErrorCodes errorCode, string message)
            : base(message)
        {
            ResponseStatusCode = responseStatusCode;
            ErrorCode = errorCode;
            Message = message;
        }

        // Properties
        public HttpStatusCode ResponseStatusCode { get; }

        [JsonProperty("code")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorCodes ErrorCode { get; }

        [JsonProperty]
        public new string Message { get; }
    }
}