using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet.models.testing.Entities
{
    /// <summary>
    /// Patient entity - 病人資料
    /// </summary>
    [Table("Patient")]
    public class Patient
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

        [Required]
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [Column("birthday")]
        public DateTime? Birthday { get; set; }

        [Column("gender")]
        [StringLength(10)]
        public string? Gender { get; set; }

        [Column("idNumber")]
        [StringLength(255)]
        public string? IdNumber { get; set; }

        [Column("bedNumber")]
        [StringLength(255)]
        public string? BedNumber { get; set; }

        [Column("stationId")]
        public int? StationId { get; set; }

        [Column("phone")]
        [StringLength(255)]
        public string? Phone { get; set; }

        [Column("address")]
        [StringLength(500)]
        public string? Address { get; set; }

        [Column("allergy")]
        [StringLength(1000)]
        public string? Allergy { get; set; }

        [Column("diagnosis")]
        [StringLength(1000)]
        public string? Diagnosis { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        // Navigation properties
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public virtual Station? Station { get; set; }
    }
}
