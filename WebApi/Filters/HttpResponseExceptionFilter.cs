using Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters;

public class HttpResponseExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is CountryNotFoundExceptions or DatesNotSupportedException)
        {
            // Return a 400 Bad Request for known country not found exceptions.
            context.Result = new BadRequestObjectResult(new
            {
                error = context.Exception.Message
            });
            context.ExceptionHandled = true;
        }
        else
        {
            // For any other unhandled exception, return a 500 Internal Server Error.
            context.Result = new ObjectResult(new
            {
                error = "An unexpected error occurred.",
                message = context.Exception.Message,
                trace = context.Exception.StackTrace
            })
            {
                StatusCode = 500
            };
            context.ExceptionHandled = true;
        }
    }
}