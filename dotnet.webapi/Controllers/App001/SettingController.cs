using dotnet.Core;
using dotnet.models.testing.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet.webapi.Controllers.App001
{
    [ApiController]
    [Route("app001")]
    [Authorize]
    public class SettingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SettingController> _logger;

        public SettingController(IConfiguration configuration, ILogger<SettingController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("getSetting")]
        public ActionResult<ResponseViewModel<SettingResponse>> GetSetting()
        {
            try
            {
                var setting = new SettingResponse
                {
                    AppName = _configuration.GetValue<string>("AppSettings:AppName", "ADC-STD")!,
                    Version = _configuration.GetValue<string>("AppSettings:Version", "1.0.0")!,
                    Environment = _configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT", "Production")!
                };

                return Ok(new ResponseViewModel<SettingResponse>(setting));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting settings");
                return BadRequest(new { Code = 1, Message = ex.Message });
            }
        }
    }
}
