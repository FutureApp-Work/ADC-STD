using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet.models.testing.Entities
{
    /// <summary>
    /// Station entity - 護理站
    /// </summary>
    [Table("Station")]
    public class Station
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

        // Navigation properties
        public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
    }
}
