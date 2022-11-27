using Newtonsoft.Json;
using rateit.Exceptions;
using rateit.Middlewares.Models;

namespace rateit.Middlewaress;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;
    public ExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(BaseAppException baseAppException)
        {
            await ThrowError(context, baseAppException.StatusCodeToRise, baseAppException.Errors);
        }
        catch(Exception exception)
        {
            await ThrowError(context, 500, new Dictionary<string, string[]> { { "Message", new string[] { exception.Message } } });
        }
    }

    private Task ThrowError(HttpContext context, int statusCode, Dictionary<string,string[]> errors)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = statusCode;
        return response.WriteAsync(JsonConvert.SerializeObject(ApiResponse.Failure(statusCode,errors)));
    }
}