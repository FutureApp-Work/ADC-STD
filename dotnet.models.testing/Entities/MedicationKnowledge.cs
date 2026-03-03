using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet.models.testing.Entities
{
    /// <summary>
    /// MedicationKnowledge entity - 藥品資料
    /// </summary>
    [Table("MedicationKnowledge")]
    public class MedicationKnowledge
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("createTime")]
        public DateTime CreateTime { get; set; }

        [Column("updateTime")]
        public DateTime UpdateTime { get; set; }

        [Required]
        [Column("code")]
        [StringLength(255)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [Column("scientificName")]
        [StringLength(255)]
        public string? ScientificName { get; set; }

        [Column("specification")]
        [StringLength(255)]
        public string? Specification { get; set; }

        [Column("dosageForm")]
        [StringLength(255)]
        public string? DosageForm { get; set; }

        [Column("commonAmount")]
        [Precision(18, 2)]
        public decimal CommonAmount { get; set; }

        [Column("regulatoryLevel")]
        [StringLength(50)]
        public string? RegulatoryLevel { get; set; }

        [Column("isColdStore")]
        public bool IsColdStore { get; set; }

        [Column("isHighAlert")]
        public bool IsHighAlert { get; set; }

        [Column("isLASA")]
        public bool IsLASA { get; set; }

        [Column("isControl")]
        public bool IsControl { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("barcode")]
        [StringLength(255)]
        public string? Barcode { get; set; }

        // Navigation properties
        public virtual ICollection<ActionHistory> ActionHistories { get; set; } = new List<ActionHistory>();
        public virtual ICollection<Storage> Storages { get; set; } = new List<Storage>();
    }
}
