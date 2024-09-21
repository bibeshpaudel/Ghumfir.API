using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace Ghumfir.API.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        
        _logger.LogError($"Something went wrong: {exception}");
        
        var message = exception switch
        {
            NotImplementedException => "Method not implemented",
                _ => "An unexpected error occurred."
        };
        
        await httpContext.Response.WriteAsync(message, cancellationToken);

        return true;
    }
}