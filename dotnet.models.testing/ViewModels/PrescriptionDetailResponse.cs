namespace dotnet.models.testing.ViewModels
{
    public class PrescriptionDetailResponse
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public bool IsApprove { get; set; }
        public DateTime? OpeningTime { get; set; }
        public int PatientId { get; set; }
        public string? Pharmacy { get; set; }
        public string? ReceiveNumber { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? DoctorName { get; set; }
        public string? TreatmentCategory { get; set; }
        public PatientInfo? Patient { get; set; }
        public List<PrescriptionDetailItem> Details { get; set; } = new();
    }

    public class PatientInfo
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string? BedNumber { get; set; }
    }

    public class PrescriptionDetailItem
    {
        public int Id { get; set; }
        public decimal DemandAmount { get; set; }
        public string? PrescriptionUnit { get; set; }
        public string? DoctorOrderNumber { get; set; }
        public string? DoctorOrderDetail { get; set; }
        public string? Dosage { get; set; }
        public string? Frequency { get; set; }
        public string DetailNumber { get; set; } = string.Empty;
        public string ApprovalStatus { get; set; } = string.Empty;
        public string? ApprovalUserName { get; set; }
        public DateTime? ApprovalTime { get; set; }
        public int MedicationKnowledgeId { get; set; }
        public string? MedicationName { get; set; }
        public string? MedicationCode { get; set; }
    }

    public class PrescriptionDetailRequest
    {
        public int Id { get; set; }
    }
}
