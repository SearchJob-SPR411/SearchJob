using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entity
{
    public class FavoriteVacancyEntity : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public UserEntity? User { get; set; }

        [Required]
        [ForeignKey(nameof(Vacancy))]
        public int VacancyId { get; set; }
        public VacancyEntity? Vacancy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
