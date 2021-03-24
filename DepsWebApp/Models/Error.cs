using System.Text.Json.Serialization;

namespace DepsWebApp.Models
{
    /// <summary>
    /// Custom error.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Error code.
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}