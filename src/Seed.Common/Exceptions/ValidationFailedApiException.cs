using Newtonsoft.Json;

namespace Seed.Common.Exceptions
{
    public class ValidationFailedApiException : BadRequestApiException
    {
        public ValidationFailedApiException(params string[] validationErrors)
            : base(ErrorCodes.VALIDATION_FAILED, "Validation failed")
        {
            ValidationErrors = validationErrors;
        }

        [JsonProperty]
        public string[] ValidationErrors { get; }
    }
}