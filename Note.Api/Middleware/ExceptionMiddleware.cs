using Note.Infrastructure.Log.ExceptionLoggerService;
using System.ComponentModel.DataAnnotations;

namespace Note.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IExceptionLoggerService _dbLogger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IExceptionLoggerService dbLogger)
        {
            _next = next;
            _logger = logger;
            _dbLogger = dbLogger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var statusCode = ex switch
                {
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    ValidationException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                _logger.LogError(ex, "Exception occured : Path: {Path}, Method: {Method} ", context.Request.Path, context.Request.Method);

                //string requestBody = null;
                //if (context.Request.ContentLength > 0)
                //{
                //    context.Request.EnableBuffering();
                //    using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                //    requestBody = await reader.ReadToEndAsync();
                //    context.Request.Body.Position = 0;
                //}
                //await _dbLogger.LogExceptionAsync(
                //    action: $"{context.Request.Method} {context.Request.Path}",
                //    exceptionMessage: ex.Message,
                //    stackTrace: ex.StackTrace,
                //    innerException: ex.InnerException?.ToString(),
                //    request: requestBody,
                //    userId: context.User.Identity.IsAuthenticated ? int.Parse(context.User.FindFirst("id")?.Value ?? "0") : null
                //);

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                var response = new { message = ex.Message };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
