using dotnet.models.testing.Data;
using dotnet.models.testing.Entities;
using dotnet.models.testing.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace dotnet.services.testing.Services
{
    public class AuthService : IAuthService
    {
        private readonly AdcDbContext _adcDbContext;
        private readonly ILogger<AuthService> _logger;

        public AuthService(AdcDbContext adcDbContext, ILogger<AuthService> logger)
        {
            _adcDbContext = adcDbContext;
            _logger = logger;
        }

        public async Task<UserAuthResult?> AuthenticateAsync(string account, string password)
        {
            var user = await _adcDbContext.Set<User>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Account == account && u.Password == password && u.Active);

            if (user == null) return null;

            return ToAuthResult(user);
        }

        public async Task<UserAuthResult?> AuthenticateByAccountAsync(string account)
        {
            var user = await _adcDbContext.Set<User>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Account == account && u.Active);

            if (user == null) return null;

            return ToAuthResult(user);
        }

        public async Task<UserAuthResult?> GetUserByIdAsync(int userId)
        {
            var user = await _adcDbContext.Set<User>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId && u.Active);

            if (user == null) return null;

            return ToAuthResult(user);
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _adcDbContext.Set<User>()
                .FirstOrDefaultAsync(u => u.Id == userId && u.Password == oldPassword && u.Active);

            if (user == null) return false;

            user.Password = newPassword;
            user.IsNeedToChangePassword = false;
            user.UpdateTime = DateTime.Now;

            await _adcDbContext.SaveChangesAsync();
            return true;
        }

        private static UserAuthResult ToAuthResult(User user) => new()
        {
            Id = user.Id,
            Name = user.Name,
            Account = user.Account,
            IsNeedToChangePassword = user.IsNeedToChangePassword,
            Active = user.Active
        };
    }
}
