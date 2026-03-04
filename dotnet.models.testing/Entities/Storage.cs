using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace dotnet.models.testing.Entities
{
    /// <summary>
    /// Storage entity - 庫存
    /// </summary>
    [Table("Storage")]
    public class Storage
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("createTime")]
        public DateTime CreateTime { get; set; }

        [Column("updateTime")]
        public DateTime UpdateTime { get; set; }

        [Column("amount")]
        [Precision(18, 2)]
        public decimal Amount { get; set; }

        [Column("expiryDate")]
        public DateTime? ExpiryDate { get; set; }

        [Column("lotNumber")]
        [StringLength(255)]
        public string? LotNumber { get; set; }

        [Column("medicationKnowledgeId")]
        public int MedicationKnowledgeId { get; set; }

        [Column("cabinetDrawerId")]
        public int CabinetDrawerId { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        // Navigation properties
        public virtual MedicationKnowledge MedicationKnowledge { get; set; } = null!;
        public virtual CabinetDrawer CabinetDrawer { get; set; } = null!;
    }
}
