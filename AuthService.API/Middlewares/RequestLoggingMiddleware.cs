namespace AuthService.API.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Items["X-Correlation-ID"] ?? "N/A";

            // Log request info
            _logger.LogInformation("Incoming request {Method} {Path}, CorrelationId={CorrelationId}", context.Request.Method, context.Request.Path, correlationId);

            var start = DateTime.UtcNow;
            await _next(context);
            var duration = DateTime.UtcNow - start;

            // Log response info
            _logger.LogInformation("Outgoing response {StatusCode} for {Method} {Path}, Duration={Duration}ms, CorrelationId={CorrelationId}",
                context.Response.StatusCode,
                context.Request.Method,
                context.Request.Path,
                duration.TotalMilliseconds,
                correlationId
            );
        }
    }

}
