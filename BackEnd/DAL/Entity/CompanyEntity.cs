using System.ComponentModel.DataAnnotations;

namespace DAL.Entity
{
    public class CompanyEntity : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(2000)]
        public string? Description { get; set; }
        [MaxLength(500)]
        public string? Website { get; set; }
        [MaxLength(150)]
        public string? Location { get; set; }
        [MaxLength(500)]
        public string? LogoUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public int UserId { get; set; }
        public UserEntity? User { get; set; }
        public ICollection<VacancyEntity> Vacancies { get; set; } = new List<VacancyEntity>();
    }
}
