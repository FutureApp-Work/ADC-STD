using dotnet.models.testing.ViewModels.AuthViewModels;
using dotnet.services.testing.Services;

namespace ADC_STD.ApiTests.Fixtures
{
    /// <summary>
    /// Mock implementation of IAuthService for testing
    /// </summary>
    public class MockAuthService : IAuthService
    {
        private const string TestPasswordHash = "$2a$11$X9JvKdDgJ2VbgKz7EfX.SuJwYpPf9IbOGbUj9vWmYVqX9Yw5Zt2a"; // BCrypt hash of "password123"
        
        public Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            // Simulate authentication logic
            if (request.Account == "admin" && request.Password == "password123")
            {
                return Task.FromResult<LoginResponse?>(new LoginResponse
                {
                    Token = GenerateTestJwtToken(),
                    User = new UserInfo
                    {
                        Id = 1,
                        Account = "admin",
                        Name = "Admin User",
                        Photo = "https://example.com/photo.jpg",
                        CabinetRoleId = 1,
                        PermissionCodeList = new List<string> { "patient:view", "prescription:view", "admin:full" }
                    },
                    IdleTimeout = 300
                });
            }

            // Invalid credentials
            return Task.FromResult<LoginResponse?>(null);
        }

        public Task<SettingsResponse> GetSettingsAsync()
        {
            return Task.FromResult(new SettingsResponse
            {
                Version = "1.0.0",
                IdleTimeout = 300,
                SessionTimeout = 3600,
                MaxLoginAttempts = 5,
                PasswordMinLength = 8,
                PasswordRequireUppercase = true,
                PasswordRequireLowercase = true,
                PasswordRequireDigit = true,
                PasswordRequireSpecialChar = false,
                AppName = "ADC-STD",
                SupportedLanguages = new List<string> { "en-US", "zh-TW" },
                DefaultLanguage = "zh-TW",
                MaintenanceMode = false,
                MaintenanceMessage = null
            });
        }

        public Task<bool> ValidateTokenAsync(string token)
        {
            // Simple validation - check if token has 3 parts (header.payload.signature)
            if (string.IsNullOrWhiteSpace(token))
            {
                return Task.FromResult(false);
            }

            var parts = token.Split('.');
            return Task.FromResult(parts.Length == 3);
        }

        private string GenerateTestJwtToken()
        {
            // Generate a simple JWT token format for testing
            var header = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("{\"alg\":\"HS256\",\"typ\":\"JWT\"}"));
            var payload = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("{\"sub\":\"1\",\"name\":\"Admin User\",\"iat\":1234567890}"));
            var signature = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("test-signature"));
            
            return $"{header}.{payload}.{signature}";
        }
    }
}
