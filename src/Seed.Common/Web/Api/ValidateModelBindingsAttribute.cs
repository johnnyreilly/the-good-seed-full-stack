using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Seed.Common.Exceptions;

namespace Seed.Common.Web.Api
{
    public class ValidateModelBindingsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;

            if (modelState.IsValid == false)
            {
                var errorMessages = modelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage)).ToArray();

                throw new ValidationFailedApiException(errorMessages);
            }
        }
    }
}