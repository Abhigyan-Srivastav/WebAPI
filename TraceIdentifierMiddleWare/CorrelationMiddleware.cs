namespace TraceIdentifierMiddleWare
{
    public class CorrelationMiddleware(RequestDelegate next, ILogger<CorrelationMiddleware> logger)
    {
        private const string CorrelationIdHeaderName = "X-Correlation-Id";

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers[CorrelationIdHeaderName].FirstOrDefault();
            if (string.IsNullOrEmpty(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
            }
            context.Request.Headers.TryAdd(CorrelationIdHeaderName, correlationId);
            logger.LogInformation("Request path: {RequestPath}. CorrelationId: {CorrelationId}", context.Request.Path, correlationId);
            context.Response.Headers.TryAdd(CorrelationIdHeaderName, correlationId);
            await next(context);
        }
    }
}
