using System.Text.Json.Serialization;

namespace Inventra.WebAPI.Exceptions
{
    /// <summary>
    /// RFC 7807 Problem Details response format.
    /// </summary>
    public class ProblemDetailsResponse
    {
        /// <summary>
        /// A URI reference that identifies the problem type.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// A short, human-readable summary of the problem type.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The HTTP status code.
        /// </summary>
        [JsonPropertyName("status")]
        public int Status { get; set; }

        /// <summary>
        /// A human-readable explanation specific to this occurrence.
        /// </summary>
        [JsonPropertyName("detail")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Detail { get; set; }

        /// <summary>
        /// A URI reference that identifies the specific occurrence.
        /// </summary>
        [JsonPropertyName("instance")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Instance { get; set; }

        /// <summary>
        /// Trace ID for tracking the error across logs.
        /// </summary>
        [JsonPropertyName("traceId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TraceId { get; set; }

        /// <summary>
        /// Validation errors (field-level errors).
        /// </summary>
        [JsonPropertyName("errors")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, string[]>? Errors { get; set; }

        /// <summary>
        /// Timestamp when the error occurred.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}