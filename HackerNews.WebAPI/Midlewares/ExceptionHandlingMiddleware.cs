using Microsoft.AspNetCore.Diagnostics;

namespace HackerNews.WebAPI.Midlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next, 
    ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred at path: {Path}", context.Request.Path);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex switch
            {
                ArgumentNullException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new
            {
                error = "An unexpected error occurred.",
                code = context.Response.StatusCode
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}