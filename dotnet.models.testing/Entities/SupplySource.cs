using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet.models.testing.Entities
{
    /// <summary>
    /// SupplySource entity - 供應來源
    /// </summary>
    [Table("SupplySource")]
    public class SupplySource
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

        [Column("active")]
        public bool Active { get; set; }

        // Navigation properties
        public virtual ICollection<Cabinet> Cabinets { get; set; } = new List<Cabinet>();
    }
}
