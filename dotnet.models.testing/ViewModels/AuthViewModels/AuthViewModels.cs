using System.ComponentModel.DataAnnotations;

namespace dotnet.models.testing.ViewModels.AuthViewModels
{
    /// <summary>
    /// Login request model
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// User account/username
        /// </summary>
        [Required(ErrorMessage = "Account is required")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Account must be between 1 and 255 characters")]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// User password
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Password must be between 1 and 255 characters")]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Login response data
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// JWT token for authentication
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// User information
        /// </summary>
        public UserInfo User { get; set; } = new();

        /// <summary>
        /// Idle timeout in seconds
        /// </summary>
        public int IdleTimeout { get; set; } = 300;
    }

    /// <summary>
    /// User information in login response
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// User ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User account/username
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// User display name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// User photo URL
        /// </summary>
        public string? Photo { get; set; }

        /// <summary>
        /// Cabinet role ID
        /// </summary>
        public int? CabinetRoleId { get; set; }

        /// <summary>
        /// List of permission codes
        /// </summary>
        public List<string> PermissionCodeList { get; set; } = new();
    }

    /// <summary>
    /// Settings response model
    /// </summary>
    public class SettingsResponse
    {
        /// <summary>
        /// System settings version
        /// </summary>
        public string Version { get; set; } = "1.0.0";

        /// <summary>
        /// Idle timeout in seconds
        /// </summary>
        public int IdleTimeout { get; set; } = 300;

        /// <summary>
        /// Session timeout in seconds
        /// </summary>
        public int SessionTimeout { get; set; } = 3600;

        /// <summary>
        /// Maximum login attempts before lockout
        /// </summary>
        public int MaxLoginAttempts { get; set; } = 5;

        /// <summary>
        /// Password minimum length
        /// </summary>
        public int PasswordMinLength { get; set; } = 8;

        /// <summary>
        /// Whether password requires uppercase letters
        /// </summary>
        public bool PasswordRequireUppercase { get; set; } = true;

        /// <summary>
        /// Whether password requires lowercase letters
        /// </summary>
        public bool PasswordRequireLowercase { get; set; } = true;

        /// <summary>
        /// Whether password requires digits
        /// </summary>
        public bool PasswordRequireDigit { get; set; } = true;

        /// <summary>
        /// Whether password requires special characters
        /// </summary>
        public bool PasswordRequireSpecialChar { get; set; } = false;

        /// <summary>
        /// Application name
        /// </summary>
        public string AppName { get; set; } = "ADC-STD";

        /// <summary>
        /// Supported languages
        /// </summary>
        public List<string> SupportedLanguages { get; set; } = new() { "en-US", "zh-TW" };

        /// <summary>
        /// Default language
        /// </summary>
        public string DefaultLanguage { get; set; } = "zh-TW";

        /// <summary>
        /// System maintenance mode
        /// </summary>
        public bool MaintenanceMode { get; set; } = false;

        /// <summary>
        /// Maintenance message
        /// </summary>
        public string? MaintenanceMessage { get; set; }
    }
}
