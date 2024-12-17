using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.AuthViewModel.RequesrLog;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public RequestResponseLoggingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task Invoke(HttpContext context)
    {
        // Capture the request body
        context.Request.EnableBuffering();
        var requestBodyStream = new MemoryStream();
        await context.Request.Body.CopyToAsync(requestBodyStream);
        requestBodyStream.Seek(0, SeekOrigin.Begin);
        var requestBodyText = await new StreamReader(requestBodyStream).ReadToEndAsync();
        requestBodyStream.Seek(0, SeekOrigin.Begin);
        context.Request.Body = requestBodyStream;

        // Capture the response body
        var originalResponseBodyStream = context.Response.Body;
        var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        await _next(context);

        responseBodyStream.Seek(0, SeekOrigin.Begin);
        var responseBodyText = await new StreamReader(responseBodyStream).ReadToEndAsync();
        responseBodyStream.Seek(0, SeekOrigin.Begin);

        bool isApi = context.Request.Path.StartsWithSegments("/api");
        if (isApi)
        {
            // Use a scope to resolve the scoped service
            using (var scope = _serviceProvider.CreateScope())
            {
                var requestResponseService = scope.ServiceProvider.GetRequiredService<IRequestResponseService>();

                // Log the request and response
                var log = new RequestResponseLog
                {
                    Timestamp = DateTime.Now,
                    RequestUrl = context.Request.Path,
                    HttpMethod = context.Request.Method,
                    RequestBody = requestBodyText,
                    ResponseBody = responseBodyText
                };

                await requestResponseService.AddLog(log);
            }
        }

        // Reset the response body stream
        await responseBodyStream.CopyToAsync(originalResponseBodyStream);
    }
}
