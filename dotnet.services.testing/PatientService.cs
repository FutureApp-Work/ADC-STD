using dotnet.models.testing.Data;
using dotnet.models.testing.Entities;
using dotnet.models.testing.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace dotnet.services.testing.Services
{
    public class PatientService : IPatientService
    {
        private readonly AdcDbContext _adcDbContext;
        private readonly ILogger<PatientService> _logger;

        public PatientService(AdcDbContext adcDbContext, ILogger<PatientService> logger)
        {
            _adcDbContext = adcDbContext;
            _logger = logger;
        }

        public async Task<PatientListResponse> GetPatientListAsync(PatientListRequest request)
        {
            var query = _adcDbContext.Set<Patient>().AsQueryable();

            // Apply filters
            if (request.StationId.HasValue)
            {
                query = query.Where(p => p.StationId == request.StationId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(p => p.Name.Contains(request.Name));
            }

            if (!string.IsNullOrWhiteSpace(request.BedNumber))
            {
                query = query.Where(p => p.BedNumber.Contains(request.BedNumber));
            }

            // Only active patients
            query = query.Where(p => p.Active);

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var items = await query
                .OrderBy(p => p.Name)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new PatientItem
                {
                    Id = p.Id,
                    Number = p.Number,
                    Name = p.Name,
                    Gender = p.Gender,
                    Birthday = p.Birthday != null ? p.Birthday.Value.ToString("yyyy-MM-dd") : null,
                    BedNumber = p.BedNumber,
                    StationId = p.StationId,
                    StationName = p.Station != null ? p.Station.Name : string.Empty
                })
                .ToListAsync();

            return new PatientListResponse
            {
                Items = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<PrescriptionDetailResponse?> GetPrescriptionDetailAsync(int id)
        {
            var prescription = await _adcDbContext.Set<Prescription>()
                .AsNoTracking()
                .Include(p => p.Patient)
                .Include(p => p.PrescriptionDetails)
                    .ThenInclude(d => d.MedicationKnowledge)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prescription == null)
            {
                return null;
            }

            var response = new PrescriptionDetailResponse
            {
                Id = prescription.Id,
                Number = prescription.Number,
                IsApprove = prescription.IsApprove,
                OpeningTime = prescription.OpeningTime,
                PatientId = prescription.PatientId,
                Pharmacy = prescription.Pharmacy,
                ReceiveNumber = prescription.ReceiveNumber,
                DeliveryDate = prescription.DeliveryDate,
                DoctorName = prescription.DoctorName,
                TreatmentCategory = prescription.TreatmentCategory,
                Patient = prescription.Patient != null ? new PatientInfo
                {
                    Id = prescription.Patient.Id,
                    Number = prescription.Patient.Number,
                    Name = prescription.Patient.Name,
                    Gender = prescription.Patient.Gender,
                    Birthday = prescription.Patient.Birthday,
                    BedNumber = prescription.Patient.BedNumber
                } : null,
                Details = prescription.PrescriptionDetails.Select(d => new PrescriptionDetailItem
                {
                    Id = d.Id,
                    DemandAmount = d.DemandAmount,
                    PrescriptionUnit = d.PrescriptionUnit,
                    DoctorOrderNumber = d.DoctorOrderNumber,
                    DoctorOrderDetail = d.DoctorOrderDetail,
                    Dosage = d.Dosage,
                    Frequency = d.Frequency,
                    DetailNumber = d.DetailNumber,
                    ApprovalStatus = d.ApprovalStatus,
                    ApprovalUserName = d.ApprovalUserName,
                    ApprovalTime = d.ApprovalTime,
                    MedicationKnowledgeId = d.MedicationKnowledgeId,
                    MedicationName = d.MedicationKnowledge?.Name,
                    MedicationCode = d.MedicationKnowledge?.Code
                }).ToList()
            };

            return response;
        }
    }
}
