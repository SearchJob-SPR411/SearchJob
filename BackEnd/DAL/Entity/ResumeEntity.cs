using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entity
{
    public class ResumeEntity : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required, MaxLength(150)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(2000)]
        public string? Summary { get; set; }
        [MaxLength(2000)]
        public string? Skills { get; set; }
        [MaxLength(5000)]
        public string? Experience { get; set; }
        [MaxLength(5000)]
        public string? Education { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public UserEntity? User { get; set; }
    }
}
