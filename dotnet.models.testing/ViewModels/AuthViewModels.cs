namespace dotnet.models.testing.ViewModels
{
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginUseAccountOnlyRequest
    {
        public string Username { get; set; } = string.Empty;
    }

    public class DoubleVerificationRequest
    {
        public int UserId { get; set; }
        public string VerificationCode { get; set; } = string.Empty;
    }

    public class ChangePasswordRequest
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class UserAuthResult
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public bool IsNeedToChangePassword { get; set; }
        public bool Active { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public bool IsNeedToChangePassword { get; set; }
    }

    public class DoubleVerificationResponse
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class SettingResponse
    {
        public string AppName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
    }
}
