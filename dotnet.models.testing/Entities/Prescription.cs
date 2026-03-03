using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet.models.testing.Entities
{
    /// <summary>
    /// Prescription entity - 處方
    /// </summary>
    [Table("Prescription")]
    public class Prescription
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("createTime")]
        public DateTime CreateTime { get; set; }

        [Column("updateTime")]
        public DateTime UpdateTime { get; set; }

        [Required]
        [Column("number")]
        [StringLength(255)]
        public string Number { get; set; } = string.Empty;

        [Column("isApprove")]
        public bool IsApprove { get; set; }

        [Column("openingTime")]
        public DateTime? OpeningTime { get; set; }

        [Column("patientId")]
        public int PatientId { get; set; }

        [Column("pharmacy")]
        [StringLength(255)]
        public string? Pharmacy { get; set; }

        [Column("receiveNumber")]
        [StringLength(255)]
        public string? ReceiveNumber { get; set; }

        [Column("deliveryDate")]
        public DateTime? DeliveryDate { get; set; }

        [Column("doctorOrderSequence")]
        [StringLength(255)]
        public string? DoctorOrderSequence { get; set; }

        [Column("priceUnit")]
        [StringLength(50)]
        public string? PriceUnit { get; set; }

        [Column("doctorName")]
        [StringLength(255)]
        public string? DoctorName { get; set; }

        [Column("treatmentCategory")]
        [StringLength(255)]
        public string? TreatmentCategory { get; set; }

        [Column("doctorOrderCode")]
        [StringLength(255)]
        public string? DoctorOrderCode { get; set; }

        [Column("deleteDate")]
        public DateTime? DeleteDate { get; set; }

        // Navigation properties
        public virtual Patient Patient { get; set; } = null!;
        public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; } = new List<PrescriptionDetail>();
    }
}
