using System.Net;
using System.Text.Json;
using Mohamy.Core.DTO;

namespace Mohamy.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _env = env;
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            if (context.Request.Path.StartsWithSegments("/api"))
            {
                await HandleApiExceptionAsync(context, ex);
            }
            else
            {
                await HandleMvcExceptionAsync(context, ex);
            }
        }
    }

    private async Task HandleApiExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new BaseResponse
        {
            ErrorCode = (int)HttpStatusCode.InternalServerError,
            ErrorMessage = _env.IsDevelopment() ? ex.Message : "Internal Server Error",
            Data = _env.IsDevelopment() ? ex.StackTrace : null
        };

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);

        await context.Response.WriteAsync(json);
    }

    private async Task HandleMvcExceptionAsync(HttpContext context, Exception ex)
    {
        if (context.Request.Path.StartsWithSegments("/ErrorsMvc"))
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return;
        }

        _logger.LogError(ex, "An error occurred: {Message}", ex.Message);

        string errorMessage = _env.IsDevelopment() ? ex.Message : "Internal Server Error";

        context.Response.Redirect($"/ErrorsMvc/Index?code=500&message={Uri.EscapeDataString(errorMessage)}");
    }
}
