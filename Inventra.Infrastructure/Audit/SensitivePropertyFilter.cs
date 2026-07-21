namespace Inventra.Infrastructure.Audit
{
    /// <summary>
    /// Filters sensitive properties from audit logs to prevent logging of confidential data.
    /// Uses a whitelist approach to mask sensitive field values.
    /// </summary>
    public class SensitivePropertyFilter
    {
        private readonly HashSet<string> _sensitiveProperties;

        /// <summary>
        /// Initializes a new instance of the SensitivePropertyFilter class with default sensitive properties.
        /// </summary>
        public SensitivePropertyFilter()
        {
            _sensitiveProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Password",
                "PasswordHash",
                "PasswordSalt",
                "Token",
                "AccessToken",
                "RefreshToken",
                "Secret",
                "ApiSecret",
                "ApiKey",
                "SecurityStamp",
                "ConcurrencyStamp",
                "TwoFactorEnabled",
                "AuthenticationCode",
                "VerificationCode",
                "ResetToken",
                "ConfirmationToken",
                "JwtToken",
                "AuthorizationCode",
                "OAuthToken",
                "EncryptionKey",
                "PrivateKey",
                "PublicKey",
                "Certificate",
                "SSN",
                "CreditCard",
                "CardNumber",
                "CVV",
                "PIN",
                "BankAccount",
                "RoutingNumber"
            };
        }

        /// <summary>
        /// Initializes a new instance of the SensitivePropertyFilter with custom sensitive properties.
        /// </summary>
        /// <param name="sensitiveProperties">Collection of property names to treat as sensitive.</param>
        public SensitivePropertyFilter(IEnumerable<string> sensitiveProperties)
        {
            _sensitiveProperties = new HashSet<string>(sensitiveProperties, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether a property should be filtered due to being sensitive.
        /// </summary>
        /// <param name="propertyName">Name of the property to check.</param>
        /// <returns>True if the property is sensitive; otherwise, false.</returns>
        public bool IsSensitive(string propertyName)
            => _sensitiveProperties.Contains(propertyName);

        /// <summary>
        /// Masks a sensitive property value.
        /// </summary>
        /// <param name="value">Original value to mask.</param>
        /// <returns>Masked value representation.</returns>
        public static string MaskValue(string? value)
            => string.IsNullOrEmpty(value) ? "***" : "***";

        /// <summary>
        /// Filters a property value, masking it if the property is sensitive.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">Value to filter.</param>
        /// <returns>Original value if not sensitive; masked value if sensitive.</returns>
        public string FilterValue(string propertyName, string? value)
            => IsSensitive(propertyName) ? MaskValue(value) : value ?? string.Empty;

        /// <summary>
        /// Adds a custom property to the sensitive properties list.
        /// </summary>
        /// <param name="propertyName">Name of the property to mark as sensitive.</param>
        public void AddSensitiveProperty(string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
                _sensitiveProperties.Add(propertyName);
        }

        /// <summary>
        /// Removes a property from the sensitive properties list.
        /// </summary>
        /// <param name="propertyName">Name of the property to remove.</param>
        public void RemoveSensitiveProperty(string propertyName)
            => _sensitiveProperties.Remove(propertyName);
    }
}