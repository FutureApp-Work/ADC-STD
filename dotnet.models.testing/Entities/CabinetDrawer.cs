using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet.models.testing.Entities
{
    /// <summary>
    /// CabinetDrawer entity - 藥櫃抽屜
    /// </summary>
    [Table("CabinetDrawer")]
    public class CabinetDrawer
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("createTime")]
        public DateTime CreateTime { get; set; }

        [Column("updateTime")]
        public DateTime UpdateTime { get; set; }

        [Required]
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [Column("cabinetId")]
        public int CabinetId { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        // Navigation properties
        public virtual Cabinet Cabinet { get; set; } = null!;
    }
}
