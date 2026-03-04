using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet.models.testing.Entities
{
    /// <summary>
    /// ActionHistory entity - 操作記錄
    /// </summary>
    [Table("ActionHistory")]
    public class ActionHistory
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("createTime")]
        public DateTime CreateTime { get; set; }

        [Column("updateTime")]
        public DateTime UpdateTime { get; set; }

        [Column("action")]
        [StringLength(255)]
        public string? Action { get; set; }

        [Column("userId")]
        public int? UserId { get; set; }

        [Column("medicationKnowledgeId")]
        public int? MedicationKnowledgeId { get; set; }

        [Column("description")]
        [StringLength(1000)]
        public string? Description { get; set; }

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual MedicationKnowledge? MedicationKnowledge { get; set; }
    }
}
