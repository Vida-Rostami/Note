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
                switch (ex)
                {
                    case KeyNotFoundException: context.Response.StatusCode = 404; break;
                    case ValidationException: context.Response.StatusCode = 400; break;
                    default: context.Response.StatusCode = 500; break;
                }
                //context.Response.StatusCode = 500;
                context.Response.ContentType  = "application/json";
                var response = new { message = ex.Message };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
