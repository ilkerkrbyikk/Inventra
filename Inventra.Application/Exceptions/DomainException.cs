namespace Inventra.Application.Exceptions
{
    /// <summary>
    /// Exception thrown when a domain business rule is violated.
    /// Should be caught and converted to Result.Failure in handlers.
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// Gets the error code for this domain exception.
        /// </summary>
        public string? ErrorCode { get; }

        /// <summary>
        /// Initializes a new instance of the DomainException class.
        /// </summary>
        /// <param name="message">Error message describing the business rule violation.</param>
        public DomainException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DomainException class with error code.
        /// </summary>
        /// <param name="message">Error message describing the business rule violation.</param>
        /// <param name="errorCode">Error code for structured error handling.</param>
        public DomainException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the DomainException class with inner exception.
        /// </summary>
        /// <param name="message">Error message describing the business rule violation.</param>
        /// <param name="innerException">Inner exception that caused this exception.</param>
        public DomainException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}