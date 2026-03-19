using dotnet.Core;
using dotnet.models.testing.ViewModels;
using dotnet.services.testing.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace dotnet.webapi.Controllers.App001
{
    [ApiController]
    [Route("app001")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _authService = authService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseViewModel<LoginResponse>>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _authService.AuthenticateAsync(request.Username, request.Password);
                if (user == null)
                {
                    return Unauthorized(new { Code = 401, Message = "Invalid account or password" });
                }

                var token = GenerateJwtToken(user);
                return Ok(new ResponseViewModel<LoginResponse>(new LoginResponse
                {
                    Token = token,
                    UserId = user.Id,
                    Name = user.Name,
                    Account = user.Account,
                    IsNeedToChangePassword = user.IsNeedToChangePassword
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return BadRequest(new { Code = 1, Message = ex.Message });
            }
        }

        [HttpPost("loginUseAccountOnly")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseViewModel<LoginResponse>>> LoginUseAccountOnly([FromBody] LoginUseAccountOnlyRequest request)
        {
            try
            {
                var user = await _authService.AuthenticateByAccountAsync(request.Username);
                if (user == null)
                {
                    return Unauthorized(new { Code = 401, Message = "Account not found or inactive" });
                }

                var token = GenerateJwtToken(user);
                return Ok(new ResponseViewModel<LoginResponse>(new LoginResponse
                {
                    Token = token,
                    UserId = user.Id,
                    Name = user.Name,
                    Account = user.Account,
                    IsNeedToChangePassword = user.IsNeedToChangePassword
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during SSO login");
                return BadRequest(new { Code = 1, Message = ex.Message });
            }
        }

        [HttpPost("doubleVerification")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseViewModel<DoubleVerificationResponse>>> DoubleVerification([FromBody] DoubleVerificationRequest request)
        {
            try
            {
                var user = await _authService.GetUserByIdAsync(request.UserId);
                if (user == null)
                {
                    return Unauthorized(new { Code = 401, Message = "User not found or inactive" });
                }

                // Verification code validation placeholder — extend with actual 2FA logic
                if (string.IsNullOrWhiteSpace(request.VerificationCode))
                {
                    return BadRequest(new { Code = 400, Message = "Verification code is required" });
                }

                var token = GenerateJwtToken(user);
                return Ok(new ResponseViewModel<DoubleVerificationResponse>(new DoubleVerificationResponse
                {
                    Token = token,
                    UserId = user.Id,
                    Name = user.Name
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during double verification");
                return BadRequest(new { Code = 1, Message = ex.Message });
            }
        }

        [HttpPost("changePassword")]
        [Authorize]
        public async Task<ActionResult<ResponseViewModel<bool>>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var success = await _authService.ChangePasswordAsync(request.UserId, request.OldPassword, request.NewPassword);
                if (!success)
                {
                    return BadRequest(new { Code = 400, Message = "Invalid user or old password" });
                }

                return Ok(new ResponseViewModel<bool>(true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password");
                return BadRequest(new { Code = 1, Message = ex.Message });
            }
        }

        private string GenerateJwtToken(UserAuthResult user)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddMinutes(jwtSection.GetValue<int>("ExpiryMinutes", 480));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Account),
                new Claim("name", user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
