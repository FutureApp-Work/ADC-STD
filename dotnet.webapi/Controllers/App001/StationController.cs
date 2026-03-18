using dotnet.Core;
using dotnet.models.testing.ViewModels;
using dotnet.services.testing.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet.webapi.Controllers.App001
{
    [ApiController]
    [Route("app001")]
    [Authorize]
    public class StationController : ControllerBase
    {
        private readonly IStationService _stationService;
        private readonly ILogger<StationController> _logger;

        public StationController(IStationService stationService, ILogger<StationController> logger)
        {
            _stationService = stationService;
            _logger = logger;
        }

        [HttpGet("getStationList")]
        public async Task<ActionResult<ResponseViewModel<StationListResponse>>> GetStationList(
            [FromQuery] string? name,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var request = new StationListRequest
                {
                    Name = name,
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _stationService.GetStationListAsync(request);
                return Ok(new ResponseViewModel<StationListResponse>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting station list");
                return BadRequest(new { Code = 1, Message = ex.Message });
            }
        }
    }
}
