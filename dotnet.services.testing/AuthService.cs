using dotnet.Core;
using dotnet.models.testing.Data;
using dotnet.models.testing.Entities;
using dotnet.models.testing.ViewModels.AuthViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace dotnet.services.testing.Services
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticate user and generate JWT token
        /// </summary>
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        
        /// <summary>
        /// Get system settings
        /// </summary>
        Task<SettingsResponse> GetSettingsAsync();
        
        /// <summary>
        /// Validate JWT token
        /// </summary>
        Task<bool> ValidateTokenAsync(string token);
    }

    /// <summary>
    /// Authentication service implementation
    /// </summary>
    public class AuthService : IAuthService, IOperationScoped
    {
        private readonly AdcDbContext _adcDbContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            AdcDbContext adcDbContext,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _adcDbContext = adcDbContext;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Authenticate user and generate JWT token
        /// </summary>
        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Login attempt for account: {Account}", request.Account);

                // Find user by account
                var user = await _adcDbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Account == request.Account && u.Active);

                if (user == null)
                {
                    _logger.LogWarning("Login failed: User not found or inactive for account: {Account}", request.Account);
                    return null;
                }

                // Verify password using BCrypt
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                {
                    _logger.LogWarning("Login failed: Invalid password for account: {Account}", request.Account);
                    return null;
                }

                // Generate JWT token
                var token = GenerateJwtToken(user);

                // Get user permissions (placeholder - would query from DB in real implementation)
                var permissions = await GetUserPermissionsAsync(user.Id);

                // Get idle timeout from configuration
                var idleTimeout = _configuration.GetValue<int>("JwtSettings:IdleTimeoutSeconds", 300);

                _logger.LogInformation("Login successful for account: {Account}, UserId: {UserId}", request.Account, user.Id);

                return new LoginResponse
                {
                    Token = token,
                    User = new UserInfo
                    {
                        Id = user.Id,
                        Account = user.Account,
                        Name = user.Name,
                        Photo = user.Photo,
                        CabinetRoleId = user.CabinetRoleId,
                        PermissionCodeList = permissions
                    },
                    IdleTimeout = idleTimeout
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for account: {Account}", request.Account);
                throw;
            }
        }

        /// <summary>
        /// Get system settings
        /// </summary>
        public Task<SettingsResponse> GetSettingsAsync()
        {
            // Load settings from configuration
            var settings = new SettingsResponse
            {
                Version = _configuration.GetValue<string>("AppSettings:Version", "1.0.0"),
                IdleTimeout = _configuration.GetValue<int>("JwtSettings:IdleTimeoutSeconds", 300),
                SessionTimeout = _configuration.GetValue<int>("JwtSettings:SessionTimeoutSeconds", 3600),
                MaxLoginAttempts = _configuration.GetValue<int>("AppSettings:MaxLoginAttempts", 5),
                PasswordMinLength = _configuration.GetValue<int>("AppSettings:PasswordMinLength", 8),
                PasswordRequireUppercase = _configuration.GetValue<bool>("AppSettings:PasswordRequireUppercase", true),
                PasswordRequireLowercase = _configuration.GetValue<bool>("AppSettings:PasswordRequireLowercase", true),
                PasswordRequireDigit = _configuration.GetValue<bool>("AppSettings:PasswordRequireDigit", true),
                PasswordRequireSpecialChar = _configuration.GetValue<bool>("AppSettings:PasswordRequireSpecialChar", false),
                AppName = _configuration.GetValue<string>("AppSettings:AppName", "ADC-STD"),
                SupportedLanguages = _configuration.GetSection("AppSettings:SupportedLanguages").Get<List<string>>() ?? new List<string> { "en-US", "zh-TW" },
                DefaultLanguage = _configuration.GetValue<string>("AppSettings:DefaultLanguage", "zh-TW"),
                MaintenanceMode = _configuration.GetValue<bool>("AppSettings:MaintenanceMode", false),
                MaintenanceMessage = _configuration.GetValue<string>("AppSettings:MaintenanceMessage", null)
            };

            return Task.FromResult(settings);
        }

        /// <summary>
        /// Validate JWT token
        /// </summary>
        public Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(GetJwtSecret());

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = GetJwtIssuer(),
                    ValidateAudience = true,
                    ValidAudience = GetJwtAudience(),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Generate JWT token for user
        /// </summary>
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(GetJwtSecret());
            var expirationHours = _configuration.GetValue<int>("JwtSettings:TokenExpirationHours", 8);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Account),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("CabinetRoleId", user.CabinetRoleId?.ToString() ?? "0")
            };

            // Add role claim if cabinetRoleId exists
            if (user.CabinetRoleId.HasValue)
            {
                claims.Add(new Claim(ClaimTypes.Role, $"Role_{user.CabinetRoleId.Value}"));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(expirationHours),
                Issuer = GetJwtIssuer(),
                Audience = GetJwtAudience(),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Get user permissions from database
        /// </summary>
        private async Task<List<string>> GetUserPermissionsAsync(int userId)
        {
            // TODO: Implement permission retrieval from database
            // For now, return default permissions based on cabinet role
            var user = await _adcDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            var permissions = new List<string>();

            if (user?.CabinetRoleId.HasValue == true)
            {
                // Add default permissions based on role
                permissions.Add("patient:view");
                permissions.Add("prescription:view");
                
                if (user.CabinetRoleId == 1) // Admin role
                {
                    permissions.Add("admin:full");
                    permissions.Add("user:manage");
                    permissions.Add("settings:manage");
                }
                else if (user.CabinetRoleId == 2) // Pharmacist role
                {
                    permissions.Add("medication:manage");
                    permissions.Add("prescription:approve");
                }
            }

            return permissions;
        }

        private string GetJwtSecret()
        {
            var secret = _configuration.GetValue<string>("JwtSettings:Secret");
            if (string.IsNullOrEmpty(secret))
            {
                // Fallback to a default secret for development (should be changed in production)
                secret = "your-super-secret-key-that-is-32-chars-long!";
            }
            return secret;
        }

        private string GetJwtIssuer()
        {
            return _configuration.GetValue<string>("JwtSettings:Issuer", "ADC-STD");
        }

        private string GetJwtAudience()
        {
            return _configuration.GetValue<string>("JwtSettings:Audience", "ADC-STD-Client");
        }
    }
}
