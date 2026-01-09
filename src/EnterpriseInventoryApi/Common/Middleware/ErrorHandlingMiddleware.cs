using System.Net;
using System.Text.Json;
using EnterpriseInventoryApi.Common.Errors;

namespace EnterpriseInventoryApi.Common.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ApiException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";
            var response = ErrorResponse.FromException(ex.Code, ex.Message, context.TraceIdentifier);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var response = ErrorResponse.FromException(ErrorCodes.ServerError, "Unexpected error", context.TraceIdentifier);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
