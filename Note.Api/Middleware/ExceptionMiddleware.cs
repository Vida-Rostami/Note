using Note.Domain;
using Note.Domain.Log;
using Note.Infrastructure.Log.ExceptionLoggerService;
using System.ComponentModel.DataAnnotations;

namespace Note.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, IExceptionLogger dbLogger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var traceId = context.TraceIdentifier;
                await dbLogger.LogException(new ExceptionLog
                {
                    Action = context.Request.Path,
                    ExceptionMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.Message,
                    Request = context.Request.Method,
                    UserId = null,
                    TraceId = traceId
                });
                var (statusCode, message) = ex switch
                {
                    KeyNotFoundException => (StatusCodes.Status404NotFound, ErrorMessages.NotFound),
                    ValidationException => (StatusCodes.Status400BadRequest, ErrorMessages.BadRequest),
                    _ => (StatusCodes.Status500InternalServerError , ErrorMessages.InternalServerError)
                };
                //switch (ex)
                //{
                //    case KeyNotFoundException: context.Response.StatusCode = 404; break;
                //    case ValidationException: context.Response.StatusCode = 400; break;
                //    default: context.Response.StatusCode = 500; break;
                //}
               context.Response.StatusCode = statusCode;
                context.Response.ContentType  = "application/json";
                //var response = new { message = ex.Message };
                //await context.Response.WriteAsJsonAsync(response);
                await context.Response.WriteAsJsonAsync(new
                {
                    message
                });
            }
        }
    }
}
