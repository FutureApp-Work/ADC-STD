namespace dotnet.models.testing.ViewModels
{
    public class PatientListResponse
    {
        public List<PatientItem> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

    public class PatientItem
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public string? Birthday { get; set; }
        public string? BedNumber { get; set; }
        public int? StationId { get; set; }
        public string? StationName { get; set; }
    }

    public class PatientListRequest
    {
        public int? StationId { get; set; }
        public string? Name { get; set; }
        public string? BedNumber { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
