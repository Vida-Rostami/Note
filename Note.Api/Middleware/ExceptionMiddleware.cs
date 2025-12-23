using Note.Domain;
using System.ComponentModel.DataAnnotations;

namespace Note.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
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
