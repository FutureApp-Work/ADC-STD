using dotnet.models.testing.Data;
using dotnet.models.testing.Entities;
using dotnet.models.testing.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace dotnet.services.testing.Services
{
    public class StationService : IStationService
    {
        private readonly AdcDbContext _adcDbContext;
        private readonly ILogger<StationService> _logger;

        public StationService(AdcDbContext adcDbContext, ILogger<StationService> logger)
        {
            _adcDbContext = adcDbContext;
            _logger = logger;
        }

        public async Task<StationListResponse> GetStationListAsync(StationListRequest request)
        {
            var query = _adcDbContext.Set<Station>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(s => s.Name.Contains(request.Name));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(s => s.Name)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(s => new StationListItem
                {
                    Id = s.Id,
                    Code = s.Code,
                    Name = s.Name
                })
                .ToListAsync();

            return new StationListResponse
            {
                Items = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
