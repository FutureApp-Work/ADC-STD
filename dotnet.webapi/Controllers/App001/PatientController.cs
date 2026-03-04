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
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<PatientController> _logger;

        public PatientController(IPatientService patientService, ILogger<PatientController> logger)
        {
            _patientService = patientService;
            _logger = logger;
        }

        [HttpGet("getPatientList")]
        public async Task<ActionResult<ResponseViewModel<PatientListResponse>>> GetPatientList(
            [FromQuery] int? stationId,
            [FromQuery] string? name,
            [FromQuery] string? bedNumber,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var request = new PatientListRequest
                {
                    StationId = stationId,
                    Name = name,
                    BedNumber = bedNumber,
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _patientService.GetPatientListAsync(request);
                return Ok(new ResponseViewModel<PatientListResponse>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting patient list");
                return BadRequest(new { Code = 1, Message = ex.Message });
            }
        }

        [HttpGet("getPrescriptionDetail")]
        public async Task<ActionResult<ResponseViewModel<PrescriptionDetailResponse>>> GetPrescriptionDetail(
            [FromQuery] int id)
        {
            try
            {
                var result = await _patientService.GetPrescriptionDetailAsync(id);
                if (result == null)
                {
                    return NotFound(new { Code = 404, Message = "Prescription not found" });
                }

                return Ok(new ResponseViewModel<PrescriptionDetailResponse>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting prescription detail");
                return BadRequest(new { Code = 1, Message = ex.Message });
            }
        }
    }
}
