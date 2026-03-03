using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet.models.testing.Entities
{
    /// <summary>
    /// BlindInventoryHistory entity - 盲盤記錄
    /// </summary>
    [Table("BlindInventoryHistory")]
    public class BlindInventoryHistory
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("createTime")]
        public DateTime CreateTime { get; set; }

        [Column("updateTime")]
        public DateTime UpdateTime { get; set; }

        [Column("inventoryTime")]
        public DateTime InventoryTime { get; set; }

        [Column("userId")]
        public int UserId { get; set; }

        [Column("cabinetDrawerId")]
        public int CabinetDrawerId { get; set; }

        [Column("description")]
        [StringLength(1000)]
        public string? Description { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
    }
}
