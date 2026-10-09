using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Enums;

namespace DAL.Entity
{
    public class JobApplicationEntity : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Vacancy))]
        public int VacancyId { get; set; }
        public VacancyEntity? Vacancy { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public UserEntity? User { get; set; }

        [ForeignKey(nameof(Resume))]
        public int? ResumeId { get; set; }
        public ResumeEntity? Resume { get; set; }

        [MaxLength(2000)]
        public string? CoverLetter { get; set; }

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
