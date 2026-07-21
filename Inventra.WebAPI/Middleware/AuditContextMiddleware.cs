using Inventra.Infrastructure.Audit;
using System.Security.Claims;

namespace Inventra.WebAPI.Middleware
{
    /// <summary>
    /// Middleware for populating audit context with request information.
    /// Extracts user ID, username, and IP address from the HTTP context.
    /// </summary>
    public class AuditContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditContextMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the AuditContextMiddleware class.
        /// </summary>
        /// <param name="next">Next middleware in the pipeline.</param>
        /// <param name="logger">Logger for recording middleware operations.</param>
        public AuditContextMiddleware(RequestDelegate next, ILogger<AuditContextMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Invokes the middleware to set up audit context for the current request.
        /// </summary>
        /// <param name="context">HTTP context for the current request.</param>
        /// <returns>Task representing the middleware execution.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Extract user information from claims
                var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var username = context.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Extract IP address
                var ipAddress = ExtractIpAddress(context);

                // Set audit context
                AuditContext.SetCurrentUser(userId, username);
                AuditContext.SetIpAddress(ipAddress);

                _logger.LogDebug(
                    "Audit context populated. UserId={UserId}, Username={Username}, IpAddress={IpAddress}",
                    userId ?? "Anonymous", username ?? "Unknown", ipAddress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error populating audit context. Continuing without audit data.");
                // Continue execution even if audit context setup fails
            }

            try
            {
                await _next(context);
            }
            finally
            {
                // Clear audit context after request processing
                AuditContext.Clear();
            }
        }

        /// <summary>
        /// Extracts the client IP address from the HTTP context.
        /// Checks for X-Forwarded-For header first (proxy scenarios), then falls back to connection remote IP.
        /// </summary>
        /// <param name="context">HTTP context.</param>
        /// <returns>IP address as string, or "Unknown" if unable to determine.</returns>
        private static string? ExtractIpAddress(HttpContext context)
        {
            // Check for X-Forwarded-For header (proxy/load balancer)
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                var ips = forwardedFor.ToString().Split(',');
                return ips.FirstOrDefault()?.Trim();
            }

            // Fallback to connection remote IP
            return context.Connection.RemoteIpAddress?.ToString();
        }
    }
}