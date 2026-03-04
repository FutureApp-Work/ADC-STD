using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet.models.testing.Entities
{
    /// <summary>
    /// User entity - 使用者
    /// </summary>
    [Table("User")]
    public class User
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

        [Required]
        [Column("account")]
        [StringLength(255)]
        public string Account { get; set; } = string.Empty;

        [Required]
        [Column("password")]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [Column("cardCode")]
        [StringLength(255)]
        public string? CardCode { get; set; }

        [Column("photo")]
        [StringLength(255)]
        public string? Photo { get; set; }

        [Column("cabinetRoleId")]
        public int? CabinetRoleId { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("hospitalArea")]
        [StringLength(255)]
        public string? HospitalArea { get; set; }

        [Column("department")]
        [StringLength(255)]
        public string? Department { get; set; }

        [Column("email")]
        [StringLength(255)]
        public string? Email { get; set; }

        [Column("isNeedToChangePassword")]
        public bool IsNeedToChangePassword { get; set; }

        // Navigation properties
        public virtual ICollection<ActionHistory> ActionHistories { get; set; } = new List<ActionHistory>();
        public virtual ICollection<BlindInventoryHistory> BlindInventoryHistories { get; set; } = new List<BlindInventoryHistory>();
    }
}
