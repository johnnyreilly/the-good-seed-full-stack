using System.Net;

namespace Seed.Common.Exceptions
{
    public class UnauthorizedApiException : ApiException
    {
        public UnauthorizedApiException(ErrorCodes errorCode, string message)
            : base(HttpStatusCode.Unauthorized, errorCode, message)
        {
        }
    }
}