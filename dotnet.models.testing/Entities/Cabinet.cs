using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet.models.testing.Entities
{
    /// <summary>
    /// Cabinet entity - 藥櫃
    /// </summary>
    [Table("Cabinet")]
    public class Cabinet
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

        [Column("departmentNote")]
        [StringLength(255)]
        public string? DepartmentNote { get; set; }

        [Column("nearExpiryDay")]
        public int NearExpiryDay { get; set; }

        [Column("isEnableLowSafeStock")]
        public bool IsEnableLowSafeStock { get; set; }

        [Column("isEnableNearExpiry")]
        public bool IsEnableNearExpiry { get; set; }

        [Column("lowSafeStockMailingTime")]
        [StringLength(10)]
        public string? LowSafeStockMailingTime { get; set; }

        [Column("nearExpiryMailingTime")]
        [StringLength(10)]
        public string? NearExpiryMailingTime { get; set; }

        [Column("closeExpiryMailingTime")]
        [StringLength(10)]
        public string? CloseExpiryMailingTime { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("supplySourceId")]
        public int? SupplySourceId { get; set; }

        // Navigation properties
        public virtual ICollection<CabinetDrawer> CabinetDrawers { get; set; } = new List<CabinetDrawer>();
        public virtual SupplySource? SupplySource { get; set; }
    }
}
