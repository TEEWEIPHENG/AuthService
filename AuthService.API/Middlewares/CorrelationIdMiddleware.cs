namespace AuthService.API.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private const string HeaderKey = "X-Correlation-ID";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(HeaderKey, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers[HeaderKey] = correlationId;
            }

            context.Response.Headers[HeaderKey] = correlationId;
            context.Items[HeaderKey] = correlationId;

            await _next(context);
        }
    }
}
