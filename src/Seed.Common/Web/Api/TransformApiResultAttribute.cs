using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Seed.Common.Exceptions;
using Seed.Common.Logging;

namespace Seed.Common.Web.Api
{
    public class TransformApiResultAttribute : ActionFilterAttribute
    {
        // Instance Variables
        private readonly ILoggingService _loggingService;

        // C'tor
        public TransformApiResultAttribute(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        // ActionFilterAttribute Override
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var result = context.Result as ObjectResult;
            var exception = context.Exception;

            if (result != null)
            {
                if (result.Value as StreamContent != null)
                {
                    var streamContent = (StreamContent) result.Value;
                    var stream = streamContent.ReadAsStringAsync().Result;
                    var streamLength = stream.Length;

                    var contentTypeText = GetContentType(streamContent);

                    context.Result = new ContentResult {Content = stream, ContentType = contentTypeText};
                    return;
                }

                if (result.Value is IApiResult)
                    return;

                if (result is BadRequestObjectResult || result is NotFoundObjectResult)
                    result.Value = ApiResult.FromError(result.Value);
                else
                    result.Value = ApiResult.From(result.Value);
            }
            else if (exception != null && context.ExceptionHandled == false)
            {
                var apiException = exception as ApiException;

                if (apiException == null)
                {
                    _loggingService.Error(exception,
                        $"Transforming unhandled '{exception.GetType()}' to an InternalServerErrorApiException");

                    apiException = new InternalServerErrorApiException();
                }

                context.Result = new ObjectResult(ApiResult.FromError(apiException));
                context.HttpContext.Response.StatusCode = (int) apiException.ResponseStatusCode;
                context.ExceptionHandled = true;
            }
        }

        private static string GetContentType(StreamContent streamContent)
        {
            var contentTypeItems = new List<string>();
            if (!string.IsNullOrWhiteSpace(streamContent.Headers?.ContentType?.MediaType))
                contentTypeItems.Add(streamContent.Headers.ContentType.MediaType);
            if (!string.IsNullOrWhiteSpace(streamContent.Headers?.ContentType?.CharSet))
                contentTypeItems.Add(streamContent.Headers.ContentType.CharSet);
            var contentText = string.Join("; ", contentTypeItems);
            return contentText;
        }
    }
}