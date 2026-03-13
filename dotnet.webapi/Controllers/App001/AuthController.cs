using dotnet.Core;
using dotnet.models.testing.ViewModels.AuthViewModels;
using dotnet.services.testing.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet.webapi.Controllers.App001
{
    /// <summary>
    /// Authentication controller for user login and settings
    /// </summary>
    [ApiController]
    [Route("app001")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// User login
        /// </summary>
        /// <remarks>
        /// Authenticates a user with account and password, returns JWT token and user information.
        /// 
        /// Sample request:
        ///     POST /app001/login
        ///     {
        ///         "account": "admin",
        ///         "password": "password123"
        ///     }
        /// </remarks>
        /// <param name="request">Login credentials</param>
        /// <returns>JWT token, user info, and idle timeout</returns>
        /// <response code="200">Login successful</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="401">Invalid credentials</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseViewModel<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResponseViewModel<LoginResponse>>> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Validate model
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { Code = 400, Message = "Invalid request data", Errors = ModelState });
                }

                _logger.LogInformation("Login attempt for account: {Account}", request.Account);

                // Attempt login
                var result = await _authService.LoginAsync(request);

                if (result == null)
                {
                    _logger.LogWarning("Login failed for account: {Account}", request.Account);
                    return Unauthorized(new { Code = 401, Message = "Invalid account or password" });
                }

                _logger.LogInformation("Login successful for account: {Account}", request.Account);
                return Ok(new ResponseViewModel<LoginResponse>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for account: {Account}", request.Account);
                return BadRequest(new { Code = 500, Message = "An error occurred during login" });
            }
        }

        /// <summary>
        /// Get system settings
        /// </summary>
        /// <remarks>
        /// Retrieves system-wide settings including timeouts, password policies, and application configuration.
        /// This endpoint can be accessed without authentication to get initial configuration.
        /// 
        /// Sample request:
        ///     GET /app001/getSetting
        /// </remarks>
        /// <returns>System settings</returns>
        /// <response code="200">Settings retrieved successfully</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("getSetting")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseViewModel<SettingsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseViewModel<SettingsResponse>>> GetSetting()
        {
            try
            {
                _logger.LogInformation("Getting system settings");

                var settings = await _authService.GetSettingsAsync();

                return Ok(new ResponseViewModel<SettingsResponse>(settings));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting system settings");
                return BadRequest(new { Code = 500, Message = "An error occurred while retrieving settings" });
            }
        }

        /// <summary>
        /// Validate JWT token
        /// </summary>
        /// <remarks>
        /// Validates if the provided JWT token is still valid.
        /// </remarks>
        /// <param name="token">JWT token to validate</param>
        /// <returns>Validation result</returns>
        /// <response code="200">Token is valid</response>
        /// <response code="400">Invalid request</response>
        /// <response code="401">Token is invalid or expired</response>
        [HttpPost("validateToken")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseViewModel<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResponseViewModel<bool>>> ValidateToken([FromBody] string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    return BadRequest(new { Code = 400, Message = "Token is required" });
                }

                var isValid = await _authService.ValidateTokenAsync(token);

                if (!isValid)
                {
                    return Unauthorized(new { Code = 401, Message = "Token is invalid or expired" });
                }

                return Ok(new ResponseViewModel<bool>(true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                return BadRequest(new { Code = 500, Message = "An error occurred while validating token" });
            }
        }
    }
}
