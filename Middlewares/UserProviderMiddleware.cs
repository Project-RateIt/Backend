using Microsoft.AspNetCore.Authorization;
using rateit.Services;

namespace rateit.Middlewares;

public class UserProviderMiddleware
{
    private readonly RequestDelegate _next;
    
    public UserProviderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserProvider userProvider)
    {
        bool hasAuthorizations = context.GetEndpoint()!.Metadata.Any(c => c.GetType() == typeof(AuthorizeAttribute));
        if (!hasAuthorizations)
        {
            await _next(context);
        }
        else
        {
            userProvider.SetUser(Guid.Parse(context.User.Claims.First(c => c.Type == "Id").Value));
            await _next(context);
        }
    }
}