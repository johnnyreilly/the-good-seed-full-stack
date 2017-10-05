using System.Net;

namespace Seed.Common.Exceptions
{
    public class BadRequestApiException : ApiException
    {
        public BadRequestApiException(ErrorCodes errorCode, string message)
            : base(HttpStatusCode.BadRequest, errorCode, message)
        {
        }
    }
}