namespace Inventra.WebAPI.Exceptions
{
    /// <summary>
    /// Context information captured during exception handling.
    /// </summary>
    public class ExceptionInfo
    {
        /// <summary>
        /// Unique identifier for this exception occurrence.
        /// </summary>
        public string TraceId { get; set; } = string.Empty;

        /// <summary>
        /// Type of the exception that occurred.
        /// </summary>
        public string ExceptionType { get; set; } = string.Empty;

        /// <summary>
        /// HTTP status code to return.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// User-friendly error message.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Detailed error description (hidden in production).
        /// </summary>
        public string? Detail { get; set; }

        /// <summary>
        /// Collection of validation errors (if applicable).
        /// </summary>
        public Dictionary<string, string[]>? Errors { get; set; }

        /// <summary>
        /// Timestamp when the exception occurred.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}