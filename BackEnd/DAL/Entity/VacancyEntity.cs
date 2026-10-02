using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entity
{
    public enum EmploymentType
    {
        FullTime,
        PartTime,
        Contract,
        Internship
    }

    public enum WorkFormat
    {
        Office,
        Remote,
        Hybrid
    }

    public enum VacancyStatus
    {
        Draft,
        Active,
        Paused,
        Closed
    }

    public class VacancyEntity : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int CompanyId { get; set; }

        public CompanyEntity? Company { get; set; }

        [Required, MaxLength(150)]
        public string Location { get; set; } = string.Empty;

        [Required]
        public EmploymentType EmploymentType { get; set; }

        [Required]
        public WorkFormat WorkFormat { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SalaryMin { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SalaryMax { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;
        public VacancyStatus Status { get; set; } = VacancyStatus.Draft;
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public UserEntity? User { get; set; }
    }
}