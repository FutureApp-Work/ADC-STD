using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet.models.testing.Entities
{
    /// <summary>
    /// PrescriptionDetail entity - 處方明細
    /// </summary>
    [Table("PrescriptionDetail")]
    public class PrescriptionDetail
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("createTime")]
        public DateTime CreateTime { get; set; }

        [Column("updateTime")]
        public DateTime UpdateTime { get; set; }

        [Column("demandAmount")]
        [Precision(18, 2)]
        public decimal DemandAmount { get; set; }

        [Column("prescriptionUnit")]
        [StringLength(50)]
        public string? PrescriptionUnit { get; set; }

        [Column("projectedDestroyAmount")]
        [StringLength(50)]
        public string? ProjectedDestroyAmount { get; set; }

        [Column("doctorOrderNumber")]
        [StringLength(255)]
        public string? DoctorOrderNumber { get; set; }

        [Column("doctorOrderDetail")]
        [StringLength(1000)]
        public string? DoctorOrderDetail { get; set; }

        [Column("lastTimeToGet")]
        public DateTime? LastTimeToGet { get; set; }

        [Column("patientId")]
        public int PatientId { get; set; }

        [Column("prescriptionId")]
        public int PrescriptionId { get; set; }

        [Column("medicationKnowledgeId")]
        public int MedicationKnowledgeId { get; set; }

        [Column("dailyAverageAmountDescription")]
        [StringLength(1000)]
        public string? DailyAverageAmountDescription { get; set; }

        [Column("hisAmount")]
        [StringLength(50)]
        public string? HisAmount { get; set; }

        [Column("dosage")]
        [StringLength(255)]
        public string? Dosage { get; set; }

        [Column("frequency")]
        [StringLength(255)]
        public string? Frequency { get; set; }

        [Required]
        [Column("detailNumber")]
        [StringLength(255)]
        public string DetailNumber { get; set; } = string.Empty;

        [Column("approvalStatus")]
        [StringLength(10)]
        public string ApprovalStatus { get; set; } = "Y";

        [Column("approvalUserAccount")]
        [StringLength(255)]
        public string? ApprovalUserAccount { get; set; }

        [Column("approvalUserName")]
        [StringLength(255)]
        public string? ApprovalUserName { get; set; }

        [Column("approvalTime")]
        public DateTime? ApprovalTime { get; set; }

        // Navigation properties
        public virtual Patient Patient { get; set; } = null!;
        public virtual Prescription Prescription { get; set; } = null!;
        public virtual MedicationKnowledge MedicationKnowledge { get; set; } = null!;
    }
}
