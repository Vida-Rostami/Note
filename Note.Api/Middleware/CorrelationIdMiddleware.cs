namespace Note.Api.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers.ContainsKey("X-Correlation-Id")
                ? context.Request.Headers["X-Correlation-Id"].ToString() : Guid.NewGuid().ToString();

            context.TraceIdentifier = correlationId;
            context.Response.Headers["X-Correlation-Id"] = correlationId;
            await _next(context);
        }
    }
}
