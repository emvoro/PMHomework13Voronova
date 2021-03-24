using System.Text.Json.Serialization;

namespace DepsWebApp.Models
{
    /// <summary>
    /// Account model
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Account identifier.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Account login.
        /// </summary>
        [JsonPropertyName("login")]
        public string Login { get; private set; }

        /// <summary>
        /// Account password.
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; private set; }

        /// <summary>
        /// Account constructor.
        /// </summary>
        /// <param name="id">Set <see cref="Id"/>Account Base64 identifier.</param>
        /// <param name="login">Set <see cref="Login"/>Account login.</param>
        /// <param name="password">Set <see cref="Password"/>Account password.</param>
        public Account(string id, string login, string password)
        {
            Id = id;
            Login = login;
            Password = password;
        }
    }
}
