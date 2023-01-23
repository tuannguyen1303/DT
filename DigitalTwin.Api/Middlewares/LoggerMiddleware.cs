using DigitalTwin.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace DigitalTwin.Api.Middlewares
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext,
            ILogger<LoggerMiddleware> logger)
        {
            logger.LogInformation($"Path: {httpContext.Request.Path} - Method: {httpContext.Request.Method.ToUpper()}");
            await _next(httpContext);
            var exception = httpContext.Features.Get<IExceptionHandlerFeature>();
            if (exception?.Error is Exception ex)
            {
                // No log these exception
                if(ex is ResponseException || ex is TaskCanceledException || ex is OperationCanceledException)
                {
                    return;
                }                
                logger.LogError(ex, $"Url path: {httpContext.Request.Path}");
            }
        }
    }
}
