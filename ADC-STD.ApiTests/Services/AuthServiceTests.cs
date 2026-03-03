using dotnet.models.testing.Data;
using dotnet.models.testing.Entities;
using dotnet.models.testing.ViewModels.AuthViewModels;
using dotnet.services.testing.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ADC_STD.ApiTests.Services
{
    /// <summary>
    /// Unit tests for AuthService
    /// </summary>
    public class AuthServiceTests
    {
        private readonly Mock<AdcDbContext> _mockDbContext;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<AuthService>> _mockLogger;
        private readonly Mock<DbSet<User>> _mockUserDbSet;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockDbContext = new Mock<AdcDbContext>(new DbContextOptions<AdcDbContext>());
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<AuthService>>();
            _mockUserDbSet = new Mock<DbSet<User>>();

            // Setup default configuration values
            var mockJwtSettingsSection = new Mock<IConfigurationSection>();
            mockJwtSettingsSection.Setup(x => x.GetValue<string>("Secret", It.IsAny<string>())).Returns("test-secret-key-that-is-32-chars-long!");
            mockJwtSettingsSection.Setup(x => x.GetValue<string>("Issuer", It.IsAny<string>())).Returns("TestIssuer");
            mockJwtSettingsSection.Setup(x => x.GetValue<string>("Audience", It.IsAny<string>())).Returns("TestAudience");
            mockJwtSettingsSection.Setup(x => x.GetValue<int>("TokenExpirationHours", It.IsAny<int>())).Returns(8);
            mockJwtSettingsSection.Setup(x => x.GetValue<int>("IdleTimeoutSeconds", It.IsAny<int>())).Returns(300);
            mockJwtSettingsSection.Setup(x => x.GetValue<int>("SessionTimeoutSeconds", It.IsAny<int>())).Returns(3600);
            
            _mockConfiguration.Setup(x => x.GetSection("JwtSettings")).Returns(mockJwtSettingsSection.Object);
            _mockConfiguration.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>())).Returns((string key, string defaultValue) => defaultValue);
            _mockConfiguration.Setup(x => x.GetValue(It.IsAny<string>(), It.IsAny<int>())).Returns((string key, int defaultValue) => defaultValue);

            _authService = new AuthService(
                _mockDbContext.Object,
                _mockConfiguration.Object,
                _mockLogger.Object);
        }

        #region LoginAsync Tests

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsLoginResponse()
        {
            // Arrange
            var password = "password123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            
            var user = new User
            {
                Id = 1,
                Account = "admin",
                Name = "Admin User",
                Password = hashedPassword,
                Active = true,
                CabinetRoleId = 1,
                Photo = "https://example.com/photo.jpg"
            };

            var users = new List<User> { user }.AsQueryable();
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());
            
            _mockDbContext.Setup(x => x.Users).Returns(_mockUserDbSet.Object);

            var request = new LoginRequest
            {
                Account = "admin",
                Password = password
            };

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Token));
            Assert.NotNull(result.User);
            Assert.Equal(user.Id, result.User.Id);
            Assert.Equal(user.Account, result.User.Account);
            Assert.Equal(user.Name, result.User.Name);
            Assert.Equal(user.Photo, result.User.Photo);
            Assert.Equal(user.CabinetRoleId, result.User.CabinetRoleId);
            Assert.True(result.IdleTimeout > 0);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidAccount_ReturnsNull()
        {
            // Arrange
            var users = new List<User>().AsQueryable();
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());
            
            _mockDbContext.Setup(x => x.Users).Returns(_mockUserDbSet.Object);

            var request = new LoginRequest
            {
                Account = "nonexistent",
                Password = "password123"
            };

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ReturnsNull()
        {
            // Arrange
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("correctpassword");
            
            var user = new User
            {
                Id = 1,
                Account = "admin",
                Name = "Admin User",
                Password = hashedPassword,
                Active = true
            };

            var users = new List<User> { user }.AsQueryable();
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());
            
            _mockDbContext.Setup(x => x.Users).Returns(_mockUserDbSet.Object);

            var request = new LoginRequest
            {
                Account = "admin",
                Password = "wrongpassword"
            };

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_WithInactiveUser_ReturnsNull()
        {
            // Arrange
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
            
            var user = new User
            {
                Id = 1,
                Account = "admin",
                Name = "Admin User",
                Password = hashedPassword,
                Active = false
            };

            var users = new List<User> { user }.AsQueryable();
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());
            
            _mockDbContext.Setup(x => x.Users).Returns(_mockUserDbSet.Object);

            var request = new LoginRequest
            {
                Account = "admin",
                Password = "password123"
            };

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetSettingsAsync Tests

        [Fact]
        public async Task GetSettingsAsync_ReturnsSettingsResponse()
        {
            // Act
            var result = await _authService.GetSettingsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Version));
            Assert.False(string.IsNullOrEmpty(result.AppName));
            Assert.True(result.IdleTimeout > 0);
            Assert.True(result.SessionTimeout > 0);
            Assert.True(result.MaxLoginAttempts > 0);
            Assert.True(result.PasswordMinLength >= 0);
            Assert.NotNull(result.SupportedLanguages);
            Assert.False(string.IsNullOrEmpty(result.DefaultLanguage));
        }

        #endregion

        #region ValidateTokenAsync Tests

        [Fact]
        public async Task ValidateTokenAsync_WithValidToken_ReturnsTrue()
        {
            // Arrange
            var password = "password123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            
            var user = new User
            {
                Id = 1,
                Account = "admin",
                Name = "Admin User",
                Password = hashedPassword,
                Active = true
            };

            var users = new List<User> { user }.AsQueryable();
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockUserDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());
            
            _mockDbContext.Setup(x => x.Users).Returns(_mockUserDbSet.Object);

            // First generate a valid token
            var loginResult = await _authService.LoginAsync(new LoginRequest 
            { 
                Account = "admin", 
                Password = password 
            });
            
            Assert.NotNull(loginResult);

            // Act
            var isValid = await _authService.ValidateTokenAsync(loginResult.Token);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public async Task ValidateTokenAsync_WithInvalidToken_ReturnsFalse()
        {
            // Act
            var isValid = await _authService.ValidateTokenAsync("invalid.token");

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public async Task ValidateTokenAsync_WithEmptyToken_ReturnsFalse()
        {
            // Act
            var isValid = await _authService.ValidateTokenAsync("");

            // Assert
            Assert.False(isValid);
        }

        #endregion

        #region Password Hashing Tests

        [Fact]
        public void BCrypt_HashPassword_CreatesDifferentHashesForSamePassword()
        {
            // Arrange
            var password = "password123";

            // Act
            var hash1 = BCrypt.Net.BCrypt.HashPassword(password);
            var hash2 = BCrypt.Net.BCrypt.HashPassword(password);

            // Assert
            Assert.NotEqual(hash1, hash2); // BCrypt adds salt automatically
            Assert.True(BCrypt.Net.BCrypt.Verify(password, hash1));
            Assert.True(BCrypt.Net.BCrypt.Verify(password, hash2));
        }

        [Fact]
        public void BCrypt_Verify_CorrectlyValidatesPassword()
        {
            // Arrange
            var password = "password123";
            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            // Act & Assert
            Assert.True(BCrypt.Net.BCrypt.Verify(password, hash));
            Assert.False(BCrypt.Net.BCrypt.Verify("wrongpassword", hash));
        }

        #endregion
    }
}
