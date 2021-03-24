using System.Threading.Tasks;

namespace DepsWebApp.Services
{
#pragma warning disable CS1591 
    public interface IAuthService
    {
        Task<string> RegisterAsync(string login, string password);

        Task<bool> GetUserAccount(string encryptedString);
    }
#pragma warning restore CS1591
}
