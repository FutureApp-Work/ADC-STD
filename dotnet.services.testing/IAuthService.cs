using dotnet.models.testing.ViewModels;

namespace dotnet.services.testing.Services
{
    public interface IAuthService
    {
        Task<UserAuthResult?> AuthenticateAsync(string account, string password);
        Task<UserAuthResult?> AuthenticateByAccountAsync(string account);
        Task<UserAuthResult?> GetUserByIdAsync(int userId);
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
    }
}
