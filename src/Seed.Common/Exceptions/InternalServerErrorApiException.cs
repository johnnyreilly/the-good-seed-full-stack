using System.Net;

namespace Seed.Common.Exceptions
{
    public class InternalServerErrorApiException : ApiException
    {
        public InternalServerErrorApiException(string message = "An internal server error has occurred")
            : base(HttpStatusCode.InternalServerError, ErrorCodes.INTERNAL_SERVER_ERROR, message)
        {
        }
    }
}