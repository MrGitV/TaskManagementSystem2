using System.Threading.Tasks;

namespace ClientApp.Services
{
    // Defines the contract for an authentication service.
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
        void Logout();
        string GetCurrentToken();
        int? CurrentUserId { get; }
        string CurrentRole { get; }
    }
}