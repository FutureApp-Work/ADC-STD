using dotnet.models.testing.ViewModels;
using dotnet.services.testing.Services;

namespace ADC_STD.ApiTests.Fixtures;

public class MockPatientService : IPatientService
{
    private readonly List<PatientItem> _patients = new()
    {
        new PatientItem
        {
            Id = 1,
            Number = "P001",
            Name = "John Smith",
            Gender = "Male",
            Birthday = "1980-05-15",
            BedNumber = "A101",
            StationId = 1,
            StationName = "ICU"
        },
        new PatientItem
        {
            Id = 2,
            Number = "P002",
            Name = "Jane Doe",
            Gender = "Female",
            Birthday = "1990-08-22",
            BedNumber = "B205",
            StationId = 2,
            StationName = "General Ward"
        },
        new PatientItem
        {
            Id = 3,
            Number = "P003",
            Name = "Bob Johnson",
            Gender = "Male",
            Birthday = "1975-12-10",
            BedNumber = "A102",
            StationId = 1,
            StationName = "ICU"
        },
        new PatientItem
        {
            Id = 4,
            Number = "P004",
            Name = "Alice Williams",
            Gender = "Female",
            Birthday = "1985-03-30",
            BedNumber = "C301",
            StationId = 3,
            StationName = "Pediatrics"
        },
        new PatientItem
        {
            Id = 5,
            Number = "P005",
            Name = "Charlie Brown",
            Gender = "Male",
            Birthday = "1995-11-05",
            BedNumber = "B206",
            StationId = 2,
            StationName = "General Ward"
        }
    };

    private readonly List<PrescriptionDetailResponse> _prescriptions = new()
    {
        new PrescriptionDetailResponse
        {
            Id = 1,
            Number = "RX001",
            IsApprove = true,
            OpeningTime = DateTime.Now.AddDays(-2),
            PatientId = 1,
            Pharmacy = "Main Pharmacy",
            ReceiveNumber = "RCV001",
            DeliveryDate = DateTime.Now.AddDays(-1),
            DoctorName = "Dr. Smith",
            TreatmentCategory = "General",
            Patient = new PatientInfo
            {
                Id = 1,
                Number = "P001",
                Name = "John Smith",
                Gender = "Male",
                Birthday = new DateTime(1980, 5, 15),
                BedNumber = "A101"
            },
            Details = new List<PrescriptionDetailItem>
            {
                new PrescriptionDetailItem
                {
                    Id = 1,
                    DemandAmount = 10.5m,
                    PrescriptionUnit = "mg",
                    DoctorOrderNumber = "DO001",
                    DoctorOrderDetail = "Take 1 tablet daily",
                    Dosage = "10mg",
                    Frequency = "Once daily",
                    DetailNumber = "D001",
                    ApprovalStatus = "Approved",
                    ApprovalUserName = "Pharmacist Jane",
                    ApprovalTime = DateTime.Now.AddDays(-1),
                    MedicationKnowledgeId = 1,
                    MedicationName = "Aspirin",
                    MedicationCode = "ASP001"
                },
                new PrescriptionDetailItem
                {
                    Id = 2,
                    DemandAmount = 5.0m,
                    PrescriptionUnit = "mg",
                    DoctorOrderNumber = "DO002",
                    DoctorOrderDetail = "Take 1 tablet twice daily",
                    Dosage = "5mg",
                    Frequency = "Twice daily",
                    DetailNumber = "D002",
                    ApprovalStatus = "Pending",
                    MedicationKnowledgeId = 2,
                    MedicationName = "Metformin",
                    MedicationCode = "MET001"
                }
            }
        },
        new PrescriptionDetailResponse
        {
            Id = 2,
            Number = "RX002",
            IsApprove = false,
            OpeningTime = DateTime.Now.AddDays(-1),
            PatientId = 2,
            Pharmacy = "Outpatient Pharmacy",
            ReceiveNumber = "RCV002",
            DoctorName = "Dr. Johnson",
            TreatmentCategory = "Chronic",
            Patient = new PatientInfo
            {
                Id = 2,
                Number = "P002",
                Name = "Jane Doe",
                Gender = "Female",
                Birthday = new DateTime(1990, 8, 22),
                BedNumber = "B205"
            },
            Details = new List<PrescriptionDetailItem>
            {
                new PrescriptionDetailItem
                {
                    Id = 3,
                    DemandAmount = 20.0m,
                    PrescriptionUnit = "mg",
                    DoctorOrderNumber = "DO003",
                    DoctorOrderDetail = "Take as needed",
                    Dosage = "20mg",
                    Frequency = "As needed",
                    DetailNumber = "D003",
                    ApprovalStatus = "Pending",
                    MedicationKnowledgeId = 3,
                    MedicationName = "Ibuprofen",
                    MedicationCode = "IBU001"
                }
            }
        }
    };

    public Task<PatientListResponse> GetPatientListAsync(PatientListRequest request)
    {
        var query = _patients.AsQueryable();

        // Apply filters
        if (request.StationId.HasValue)
        {
            query = query.Where(p => p.StationId == request.StationId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(p => p.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(request.BedNumber))
        {
            query = query.Where(p => p.BedNumber != null && p.BedNumber.Contains(request.BedNumber, StringComparison.OrdinalIgnoreCase));
        }

        var totalCount = query.Count();
        var items = query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return Task.FromResult(new PatientListResponse
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }

    public Task<PrescriptionDetailResponse?> GetPrescriptionDetailAsync(int id)
    {
        var prescription = _prescriptions.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(prescription);
    }
}
