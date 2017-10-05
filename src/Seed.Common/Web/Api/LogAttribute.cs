using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Seed.Common.Logging;

namespace Seed.Common.Web.Api
{
    public class LogAttribute : ActionFilterAttribute
    {
        // Instance Variables
        private readonly ILoggingService _logger;


        // C'tor
        public LogAttribute(ILoggingService logger)
        {
            _logger = logger;
        }


        // ActionFilterAttribute Override
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var logger = _logger.ForContext(context.Controller.GetType());
            LogEntry(logger, context);

            var stopwatch = Stopwatch.StartNew();

            await next();

            stopwatch.Stop();

            LogExit(logger, stopwatch.Elapsed, context);
        }


        // Private Members
        private void LogEntry(ILoggingService logger, ActionExecutingContext actionContext)
        {
            try
            {
                var message = new LogMessage()
                    .Append($"Executing action {actionContext.ActionDescriptor.DisplayName} with arguments ");

                var args = "null";

                if (actionContext.ActionArguments.Count > 0)
                    args = string.Join(", ", actionContext.ActionArguments.Select(x => $"{x.Key}:{x.Value}"));

                message.Append("({Arguments})", args);

                message.Append(" - ModelState is {ValidationState}", actionContext.ModelState.ValidationState);

                logger.Information(message.MessageTemplate, parameters: message.MessageTemplateArgs);
            }
            catch (Exception ex)
            {
                logger.Warning(ex, "Failed to log entry");
            }
        }

        private void LogExit(ILoggingService logger, TimeSpan duration, ActionExecutingContext actionContext)
        {
            try
            {
                var message = new LogMessage()
                    .Append(
                        $"Executed action {actionContext.ActionDescriptor.DisplayName} in {{Duration}}ms - Response was {{ResponseCode}}",
                        duration.TotalMilliseconds, actionContext.HttpContext.Response.StatusCode);

                if (actionContext.HttpContext.Response.StatusCode == 200)
                    logger.Information(message.MessageTemplate, parameters: message.MessageTemplateArgs);
                else
                    logger.Warning(message.MessageTemplate, parameters: message.MessageTemplateArgs);
            }
            catch (Exception ex)
            {
                logger.Warning(ex, "Failed to log exit");
            }
        }


        // Nested Classes
        private class LogMessage
        {
            private readonly List<object> _args = new List<object>();

            // Instance Variables
            private readonly StringBuilder _messageTemplateBuilder = new StringBuilder();


            // C'tor

            // Properties
            public string MessageTemplate => _messageTemplateBuilder.ToString();

            public object[] MessageTemplateArgs => _args.ToArray();


            // Public Members
            public LogMessage Append(string messageTemplate, params object[] args)
            {
                _messageTemplateBuilder.Append(messageTemplate);
                _args.AddRange(args);

                return this;
            }
        }
    }
}