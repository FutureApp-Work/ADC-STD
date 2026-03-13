using System.Net;
using System.Net.Http.Json;
using ADC_STD.ApiTests.Fixtures;
using ADC_STD.ApiTests.Helpers;
using dotnet.Core;
using dotnet.models.testing.ViewModels.AuthViewModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ADC_STD.ApiTests.Controllers
{
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly HttpClient _authenticatedClient;

        public AuthControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _authenticatedClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _authenticatedClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("TestScheme");
        }

        #region POST /app001/login Tests

        [Fact]
        public async Task Login_WithValidCredentials_Returns200AndToken()
        {
            // Arrange
            var request = new LoginRequest
            {
                Account = "admin",
                Password = "password123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/app001/login", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<LoginResponse>>();
            Assert.NotNull(result);
            Assert.Equal(0, result.Code);
            Assert.NotNull(result.Data);
            Assert.False(string.IsNullOrEmpty(result.Data.Token));
            Assert.NotNull(result.Data.User);
            Assert.Equal("admin", result.Data.User.Account);
            Assert.True(result.Data.IdleTimeout > 0);
        }

        [Fact]
        public async Task Login_WithInvalidAccount_Returns401()
        {
            // Arrange
            var request = new LoginRequest
            {
                Account = "nonexistent",
                Password = "password123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/app001/login", request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithInvalidPassword_Returns401()
        {
            // Arrange
            var request = new LoginRequest
            {
                Account = "admin",
                Password = "wrongpassword"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/app001/login", request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithEmptyAccount_Returns400()
        {
            // Arrange
            var request = new LoginRequest
            {
                Account = "",
                Password = "password123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/app001/login", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithEmptyPassword_Returns400()
        {
            // Arrange
            var request = new LoginRequest
            {
                Account = "admin",
                Password = ""
            };

            // Act
            var response = await _client.PostAsJsonAsync("/app001/login", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_ResponseContainsUserInfo()
        {
            // Arrange
            var request = new LoginRequest
            {
                Account = "admin",
                Password = "password123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/app001/login", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<LoginResponse>>();
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.User);
            Assert.True(result.Data.User.Id > 0);
            Assert.False(string.IsNullOrEmpty(result.Data.User.Account));
            Assert.False(string.IsNullOrEmpty(result.Data.User.Name));
            Assert.NotNull(result.Data.User.PermissionCodeList);
        }

        [Fact]
        public async Task Login_ResponseContainsValidJwtToken()
        {
            // Arrange
            var request = new LoginRequest
            {
                Account = "admin",
                Password = "password123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/app001/login", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<LoginResponse>>();
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.False(string.IsNullOrEmpty(result.Data.Token));
            
            // Validate JWT format (should have 3 parts separated by dots)
            var tokenParts = result.Data.Token.Split('.');
            Assert.Equal(3, tokenParts.Length);
        }

        [Fact]
        public async Task Login_ResponseFormat_MatchesSpec()
        {
            // Arrange
            var request = new LoginRequest
            {
                Account = "admin",
                Password = "password123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/app001/login", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<LoginResponse>>();
            Assert.NotNull(result);
            Assert.Equal(0, result.Code);
            Assert.NotNull(result.Message);
            Assert.True(result.Timestamp > 0);
            Assert.NotNull(result.Data);
            Assert.False(string.IsNullOrEmpty(result.Data.Token));
            Assert.NotNull(result.Data.User);
            Assert.True(result.Data.IdleTimeout > 0);
        }

        #endregion

        #region GET /app001/getSetting Tests

        [Fact]
        public async Task GetSetting_Returns200()
        {
            // Act
            var response = await _client.GetAsync("/app001/getSetting");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetSetting_ReturnsSettingsData()
        {
            // Act
            var response = await _client.GetAsync("/app001/getSetting");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<SettingsResponse>>();
            Assert.NotNull(result);
            Assert.Equal(0, result.Code);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetSetting_ResponseContainsRequiredFields()
        {
            // Act
            var response = await _client.GetAsync("/app001/getSetting");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<SettingsResponse>>();
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            
            // Check required fields
            Assert.False(string.IsNullOrEmpty(result.Data.Version));
            Assert.False(string.IsNullOrEmpty(result.Data.AppName));
            Assert.True(result.Data.IdleTimeout > 0);
            Assert.True(result.Data.SessionTimeout > 0);
            Assert.NotNull(result.Data.SupportedLanguages);
            Assert.False(string.IsNullOrEmpty(result.Data.DefaultLanguage));
        }

        [Fact]
        public async Task GetSetting_ResponseContainsPasswordPolicy()
        {
            // Act
            var response = await _client.GetAsync("/app001/getSetting");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<SettingsResponse>>();
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            
            // Check password policy fields
            Assert.True(result.Data.PasswordMinLength >= 0);
            Assert.True(result.Data.MaxLoginAttempts > 0);
        }

        [Fact]
        public async Task GetSetting_ResponseFormat_MatchesSpec()
        {
            // Act
            var response = await _client.GetAsync("/app001/getSetting");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<SettingsResponse>>();
            Assert.NotNull(result);
            Assert.Equal(0, result.Code);
            Assert.NotNull(result.Message);
            Assert.True(result.Timestamp > 0);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetSetting_DoesNotRequireAuthentication()
        {
            // Act - use unauthenticated client
            var response = await _client.GetAsync("/app001/getSetting");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region POST /app001/validateToken Tests

        [Fact]
        public async Task ValidateToken_WithValidToken_Returns200()
        {
            // Arrange - first login to get a token
            var loginRequest = new LoginRequest
            {
                Account = "admin",
                Password = "password123"
            };
            var loginResponse = await _client.PostAsJsonAsync("/app001/login", loginRequest);
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<ResponseViewModel<LoginResponse>>();
            Assert.NotNull(loginResult?.Data?.Token);

            // Act
            var response = await _authenticatedClient.PostAsJsonAsync("/app001/validateToken", loginResult.Data.Token);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<bool>>();
            Assert.NotNull(result);
            Assert.Equal(0, result.Code);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task ValidateToken_WithoutAuth_Returns401()
        {
            // Arrange
            var token = "some-token";

            // Act - use unauthenticated client
            var response = await _client.PostAsJsonAsync("/app001/validateToken", token);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ValidateToken_WithEmptyToken_Returns400()
        {
            // Act
            var response = await _authenticatedClient.PostAsJsonAsync("/app001/validateToken", "");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ValidateToken_WithInvalidToken_Returns401()
        {
            // Arrange
            var invalidToken = "invalid.token.here";

            // Act
            var response = await _authenticatedClient.PostAsJsonAsync("/app001/validateToken", invalidToken);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion
    }
}
