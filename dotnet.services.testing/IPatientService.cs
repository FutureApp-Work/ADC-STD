using dotnet.models.testing.ViewModels;

namespace dotnet.services.testing.Services
{
    public interface IPatientService
    {
        Task<PatientListResponse> GetPatientListAsync(PatientListRequest request);
        Task<PrescriptionDetailResponse?> GetPrescriptionDetailAsync(int id);
    }
}
