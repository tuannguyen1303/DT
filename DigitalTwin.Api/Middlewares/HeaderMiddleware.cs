using DigitalTwin.Common.Constants;

namespace DigitalTwin.Api.Middlewares
{
    public class HeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Headers.Add(
                HttpHeaders.AccessControlExposeHeaders,
                HttpHeaders.ContentDisposition
            );
            await _next.Invoke(httpContext);
        }
    }
}