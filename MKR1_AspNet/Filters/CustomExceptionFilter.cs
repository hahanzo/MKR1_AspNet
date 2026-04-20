using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MKR2_AspNet.NewFolder
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            // Logging
            _logger.LogError("Exception occurred: {Message}. StackTrace: {StackTrace}",
                context.Exception.Message, context.Exception.StackTrace);

            var statusCode = HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred.";

            // Where resource not found
            if (context.Exception is KeyNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                message = context.Exception.Message;
            }

            // Structure response
            var response = new
            {
                Error = message,
                Details = context.Exception.InnerException?.Message,
                Timestamp = DateTime.Now,
                StatusCode = (int)statusCode
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = (int)statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
