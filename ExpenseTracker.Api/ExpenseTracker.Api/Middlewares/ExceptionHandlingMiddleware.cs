using System.Net;
using System.Text.Json;

namespace ExpenseTracker.Api.Middlewares;

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
        catch (FluentValidation.ValidationException exception)
        {
            await HandleValidationExceptionAsync(
                context,
                exception);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "An unhandled exception occurred.");

            await HandleExceptionAsync(
                context,
                exception);
        }
    }

    private static async Task HandleExceptionAsync(
    HttpContext context,
    Exception exception)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode =
            (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            isSuccess = false,
            statusCode = 500,
            error = "An unexpected error occurred."
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }

    private static async Task HandleValidationExceptionAsync(
    HttpContext context,
    FluentValidation.ValidationException exception)
    {
        context.Response.ContentType =
            "application/json";

        context.Response.StatusCode = 400;

        var response = new
        {
            isSuccess = false,
            statusCode = 400,
            error = string.Join(
                " | ",
                exception.Errors.Select(x => x.ErrorMessage))
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}