using dotnet.models.testing.ViewModels;

namespace dotnet.services.testing.Services
{
    public interface IStationService
    {
        Task<StationListResponse> GetStationListAsync(StationListRequest request);
    }
}
