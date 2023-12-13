using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the user is not authenticated
        if (!context.User.Identity.IsAuthenticated &&
            (context.Request.Path.StartsWithSegments("/Calendar") || context.Request.Path.StartsWithSegments("/Events")))
        {
            // User is not authenticated and trying to access Calendar or Event.
            // Redirect them to the login page.
            context.Response.Redirect("/Users/Login");
            return;
        }

        await _next(context);
    }
}