using Note.Domain.Log;
using Note.Infrastructure.Log.AppLogger;
using System.Diagnostics;
using System.Text;

namespace Note.Api.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, IAppLogger logger)
        {
            var stopWatch = Stopwatch.StartNew();
            context.Request.EnableBuffering();
            var requestBody = "";
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            var originalBody = context.Response.Body;
            using var newBody = new MemoryStream();
            context.Response.Body = newBody;
            await _next(context);
            stopWatch.Stop();
            newBody.Position = 0;
            var responseBody = await new StreamReader(newBody).ReadToEndAsync();
            newBody.Position = 0;
            await newBody.CopyToAsync(originalBody);
            await logger.Log(new AppLog
            {
                Action = context.GetEndpoint()?.DisplayName,
                Request = requestBody,
                Response = responseBody,
                StatusCode = context.Response.StatusCode,
                Method = context.Request.Method,
                Path = context.Request.Path,
                DurationMs = stopWatch.ElapsedMilliseconds,
                UserId = null
            });
        }
    }
}
