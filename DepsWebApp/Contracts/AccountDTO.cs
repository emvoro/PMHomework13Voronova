using System.ComponentModel.DataAnnotations;

namespace DepsWebApp.Contracts
{

    /// <summary>
    /// Transportation model for registration of new users
    /// </summary>
    public class AccountDTO
    {
        /// <summary>
        /// Received login
        /// </summary>
        [MinLength(6)]
        [Required(AllowEmptyStrings = false)]
        public string Login { get; set; }
        /// <summary>
        /// Received password
        /// </summary>
        [MinLength(6)]
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
