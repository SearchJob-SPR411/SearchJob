using System.ComponentModel.DataAnnotations;
using DAL.Enums;

namespace DAL.Entity
{
    public class UserEntity : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Phone, MaxLength(30)]
        public string? PhoneNumber { get; set; }

        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<VacancyEntity> Vacancies { get; set; } = new List<VacancyEntity>();

        public ICollection<ResumeEntity> Resumes { get; set; } = new List<ResumeEntity>();

        public SubscriptionEntity? Subscription { get; set; }
    }
}