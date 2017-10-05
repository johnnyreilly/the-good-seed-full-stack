using System.Net;

namespace Seed.Common.Exceptions
{
    public class NotFoundApiException : ApiException
    {
        public NotFoundApiException(ErrorCodes errorCode, string message)
            : base(HttpStatusCode.NotFound, errorCode, message)
        {
        }
    }
}