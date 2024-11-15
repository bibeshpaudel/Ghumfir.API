using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace Ghumfir.API.ExceptionHandlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred");

        var (statusCode, errorMessage) = exception switch
        {
            ApplicationException appEx => (StatusCodes.Status400BadRequest, appEx.Message),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "The requested resource was not found."),
            NotImplementedException => (StatusCodes.Status404NotFound, "The requested resource was not found."),
            UnauthorizedAccessException _ => (StatusCodes.Status401Unauthorized, "Unauthorized."),
            _ => (StatusCodes.Status500InternalServerError, "An internal server error occurred.")
        };

        httpContext.Response.StatusCode = statusCode;
        
        var response = new
        {
            error = new 
            {
                message = errorMessage,
                type = exception.GetType().Name
            }
        };

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}