namespace POC.FleetManager.ManagementAPI;

public class RequestLogger(RequestDelegate next, ILogger<RequestLogger> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Construct the full URL of the inbound request
        var url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
        logger.LogInformation("Inbound request URL: {Url}", url);

        // Call the next middleware in the pipeline
        await next(context);
    }
}

// Extension method to add the middleware to the pipeline
public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLogger>();
    }
}
