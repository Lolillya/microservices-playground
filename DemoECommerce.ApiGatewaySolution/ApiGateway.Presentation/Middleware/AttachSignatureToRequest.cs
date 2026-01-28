namespace ApiGateway.Presentation.Middleware
{
    public class AttachSignatureToRequest(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            // Attach a specific header to the request
            context.Request.Headers["Api-Gateway"] = "SignedByApiGateway";

            // Add ClientId header using IP address for rate limiting
            var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            context.Request.Headers["ClientId"] = clientIp;

            await next(context);
        }
    }
}
