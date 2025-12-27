using Api.Common;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Api.Middleware;

/// <summary>
/// アプリケーション全体で発生する例外をキャッチし、統一された形式でレスポンスを返すミドルウェア。
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message, errors) = exception switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest, 
                "Validation failed", 
                validationEx.Errors.Select(e => e.ErrorMessage).ToList()
            ),
            _ => (
                HttpStatusCode.InternalServerError, 
                "An internal server error occurred.", 
                new List<string> { exception.Message }
            )
        };

        context.Response.StatusCode = (int)statusCode;

        var response = ApiResponse<object>.CreateFailure(errors, message);
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        return context.Response.WriteAsync(json);
    }
}
