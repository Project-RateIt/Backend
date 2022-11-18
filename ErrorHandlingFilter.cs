using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace rateit;

public class ErrorHandlingFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        context.Result = new ObjectResult(new { error = exception.Message, inner = exception.StackTrace });
        context.HttpContext.Response.StatusCode = 500;
    }
}